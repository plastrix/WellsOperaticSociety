using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using Stripe;
using Umbraco.Web;
using WellsOperaticSociety.Models;
using Umbraco.Web.Mvc;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.Web.Models;
using LoginModel = WellsOperaticSociety.Models.MemberModels.LoginModel;

namespace WellsOperaticSociety.Web.Controllers
{
    public class MembershipSurfaceController : SurfaceController
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitLoginForm(LoginModel model)
        {
            if (ModelState.IsValid)
            {

                if (Members.Login(model.Username, model.Password))
                {
                    if (Members.GetByUsername(model.Username) != null && Members.GetByUsername(model.Username).GetPropertyValue<bool>("deactivated") == false)
                    {
                        return Redirect("/");
                    }
                    Members.Logout();
                    ModelState.AddModelError("", "Your account has been dissabled please contact us to resolve this");
                }
                else
                {
                    ModelState.AddModelError("", "There was an error with your username or password");
                }
            }
            return CurrentUmbracoPage();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Members.Logout();
            return Redirect("/");
        }

        public ActionResult ManageProfile()
        {

            var m = new Membership()
            {
                Member = Members.GetCurrentMemberId(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                MembershipType = (int)MembershipType.Ordinary
            };

            new DataManager().CreateMembership(m);

            var model = new Member(Members.GetCurrentMember());
            return PartialView("ManageProfile", model);

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitManageProfileForm(Member model)
        {
            if (ModelState.IsValid)
            {
                //No user id was passed in
                if (model.Id == 0)
                {
                    //TODO:LogError
                    ModelState.AddModelError("", "We could not find a user with that id to update");
                    return CurrentUmbracoPage();
                }

                var memberService = Services.MemberService;
                var member = memberService.GetById(model.Id);
                //couldnt find member
                if (member == null)
                {
                    //TODO:LogError
                    ModelState.AddModelError("", "We could not find a user with that id to update");
                    return CurrentUmbracoPage();
                }
                //update email if email is different
                if (model.Email != member.Email)
                {
                    //check if the new email already exists in the system
                    if (memberService.GetByEmail(model.Email) != null)
                    {
                        ModelState.AddModelError("Email", "This email address already exists in the system and emails must be unique.");
                        return CurrentUmbracoPage();
                    }
                    member.Username = model.Email;
                    member.Email = model.Email;
                }

                member.Name = model.Name;
                member.SetValue("telephoneNumber", model.TelephoneNumber);
                member.SetValue("mobileNumber", model.MobileNumber);
                member.SetValue("dateOfBirth", model.DateOfBirth);
                member.SetValue("vehicleRegistration1", model.VehicleRegistration1);
                member.SetValue("vehicleRegistration2", model.VehicleRegistration2);
                memberService.Save(member);
                //TODO: Display success message
                return RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }

        public ActionResult ManageSubscription()
        {
            var model = new ManageSubscriptionViewModel();
            try
            {
                //GetUptodate plan prices from stripe
                var planService = new StripePlanService(SensativeInformation.StripeKeys.SecretKey);
                var ordPlan = planService.Get(BusinessLogic.StaticIdentifiers.OrdinaryMemberPlanId);
                var socialPlan = planService.Get(BusinessLogic.StaticIdentifiers.SocialMemberPlanId);
                var patronPlan = planService.Get(BusinessLogic.StaticIdentifiers.PatronPlanId);
                if (ordPlan != null)
                {
                    model.OrdinaryPrice = (ordPlan.Amount/100m).ToString("N");
                }
                if (socialPlan != null)
                {
                    model.SocialPrice = (socialPlan.Amount / 100m).ToString("N");
                }
                if (patronPlan != null)
                {
                    model.PatronPrice = (patronPlan.Amount / 100m).ToString("N");
                }
            }
            catch (StripeException e)
            {
                //TODO: Log error
            }
            try
            {
                var stripeAccountId = new Member(Members.GetCurrentMember()).StripeUserId;
                if (stripeAccountId.IsNotNullOrEmpty())
                {
                    //Get plan status
                    var subscriptionService = new StripeSubscriptionService(SensativeInformation.StripeKeys.SecretKey);
                    var subs = subscriptionService.List(stripeAccountId);
                    var stripeSubscriptions = subs as StripeSubscription[] ?? subs.ToArray();
                    if (subs != null && stripeSubscriptions.Any())
                    {
                        model.IsOrdinaryMember = stripeSubscriptions.Any(m => m.StripePlan.Id == StaticIdentifiers.OrdinaryMemberPlanId && m.Status == StripeSubscriptionStatuses.Active);
                        model.IsSocialMember = stripeSubscriptions.Any(m => m.StripePlan.Id == StaticIdentifiers.SocialMemberPlanId && m.Status == StripeSubscriptionStatuses.Active);
                        model.IsPatron = stripeSubscriptions.Any(m => m.StripePlan.Id == StaticIdentifiers.PatronPlanId && m.Status == StripeSubscriptionStatuses.Active);
                    }
                }
            }
            catch(StripeException e)
            {
                //TODO: Log error
            }

            

            return PartialView("ManageSubscription", model);

        }


        public ActionResult MembershipHistory()
        {
            var dm = new BusinessLogic.DataManager();
            IEnumerable<Membership> model = dm.GetMembershipsForUser(Members.GetCurrentMemberId());
            return PartialView("MembershipHistory",model);
        }
        

        

        public ActionResult SubscriptionForm()
        {
            var planService = new StripePlanService(SensativeInformation.StripeKeys.SecretKey);
            IEnumerable<StripePlan> plans = planService.List();

            var model = new StripeCheckout();
            model.PlanId = "WOS_ORD";
            return PartialView("SubscriptionForm", model);
        }
        [HttpPost]
        public ActionResult SubmitStripeSubscriptionForm(StripeCheckout model)
        {
            if (ModelState.IsValid)
            {

                var loggedOnMember = Members.GetCurrentMember();

                var memberService = Services.MemberService;

                var member = memberService.GetById(loggedOnMember.Id);

                var stripeUserId = member.Properties.Contains("stripeUserId")
                    ? member.Properties["stripeUserId"].Value as string
                    : null;

                if (stripeUserId.IsNullOrEmpty())
                {
                    var customer = new StripeCustomerCreateOptions();
                    customer.Email = "test@email.com";
                    customer.Description = "Best customer ever!";

                    customer.SourceToken = model.StripeToken;
                    customer.PlanId = model.PlanId;
                    try
                    {
                        var customerService = new StripeCustomerService();
                        StripeCustomer stripeCustomer = customerService.Create(customer,
                            new StripeRequestOptions() {ApiKey = SensativeInformation.StripeKeys.SecretKey});
                        
                        //Log customer id on member
                        member.SetValue("stripeUserId",stripeCustomer.Id);
                        memberService.Save(member);
                        //TODO: setup membership
                        return RedirectToCurrentUmbracoPage();
                    }
                    catch (StripeException e)
                    {
                        _log.Error(e.StripeError.Message);
                        ModelState.AddModelError("",
                            "There was an error setting up your subscription. Please try again. If the issue persists please contact us");
                    }
                }
                else
                {
                    try
                    {
                        var subscriptionService = new StripeSubscriptionService();
                        StripeSubscription stripeSubscription = subscriptionService.Create(stripeUserId, model.PlanId);
                    }
                    catch (StripeException e)
                    {
                        _log.Error(e.StripeError.Message);
                        ModelState.AddModelError("",
                            "There was an error setting up your subscription. Please try again. If the issue persists please contact us");
                    }
                }
            }
            return CurrentUmbracoPage();
        }
    }
}