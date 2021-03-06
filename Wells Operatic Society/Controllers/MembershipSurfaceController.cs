﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using log4net;
using Stripe;
using Umbraco.Web;
using WellsOperaticSociety.Models;
using Umbraco.Web.Mvc;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.EmailService;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.Web.Models;
using LoginModel = WellsOperaticSociety.Models.MemberModels.LoginModel;
using Membership = WellsOperaticSociety.Models.MemberModels.Membership;

namespace WellsOperaticSociety.Web.Controllers
{
    public class MembershipSurfaceController : SurfaceController, IRenderController
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
                    var member = Members.GetByUsername(model.Username);
                    if (member != null && member.GetPropertyValue<bool>("deactivated") == false)
                    {
                        if (Roles.IsUserInRole("Member"))
                        {
                            DataManager dm = new DataManager();
                            if (!dm.DoesUserHaveCurrentMembership(member.Id))
                            {
                                  return Redirect("/manage-membership/");
                            }
                        }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForgotPasswordForm(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var member = Members.GetByEmail(model.Email);
                if (member != null)
                {

                    DataManager dm = new DataManager();
                    dm.SendResetPasswordEmail(new Member(member),ViewData,ControllerContext,TempData);
                }
                TempData["SuccessMessage"] =
                    "An email has been sent to the email address you entered. Please follow the instruction in the email to finish reseting your password";
                return RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }

        public ActionResult AuthenticateResetPassword(string token)
        {
            DataManager dm = new DataManager();
            //validate token
            var m = dm.ValidateToken(token, 1440);
            //does it exist
            //has it expired
            if (m == null)
            {
                dm.TryInvalidateToken(token);
                TempData["ErrorMessage"] = "We could not validate you on the system. This could be because the token is invalid. Please try again.";
                return PartialView("ResetPasswordForm", new ResetPassword());
            }

            ResetPassword model = new ResetPassword();
            model.Token = token;
            return PartialView("ResetPasswordForm",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                DataManager dm = new DataManager();
                var m = dm.ValidateToken(model.Token,1440);
                if (m == null)
                {
                    ModelState.AddModelError("", "There was an error whilst trying to reset your password. Please try again or contact us if the problem persists.");
                    return CurrentUmbracoPage();
                }
                var memberService = Services.MemberService;
                var member = memberService.GetById(m.Id);
                if (member != null)
                {
                    memberService.SavePassword(member,model.Password);
                    dm.TryInvalidateToken(model.Token);
                    TempData["SuccessMessage"] = "You have successfully reset your password. Now all you have to do is log in";
                    return Redirect(dm.GetLoginNode().Url);
                }
                ModelState.AddModelError("","There was an error whilst trying to reset your password. Please try again or contact us if the problem persists.");   
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
            ManageProfileViewModel model = new ManageProfileViewModel();
            model.Member = new Member(Members.GetCurrentMember());
            DataManager dm = new DataManager();
            try
            {
                model.IsSubscribedToMailingList = Task.Run(() => dm.IsUserSubscribedToMailChimpList(MailChimpListIds.MailingList, model.Member.Email)).Result; 
                model.IsSubscribedToMemberList = Task.Run(() => dm.IsUserSubscribedToMailChimpList(MailChimpListIds.Membership, model.Member.Email)).Result;
            }
            catch (Exception ex)
            {
                _log.Error("Error fetching mailchimp subscription information", ex);
            }
            return PartialView("ManageProfile", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitManageProfileForm(ManageProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                //No user id was passed in
                if (model.Member.Id == 0)
                {
                    _log.Error("No user id we submitted to the SubmitManageProfileForm on the Membership surface controller");
                    ModelState.AddModelError("", "We could not find a user with that id to update");
                    return CurrentUmbracoPage();
                }

                var memberService = Services.MemberService;
                var member = memberService.GetById(model.Member.Id);
                //couldnt find member
                if (member == null)
                {
                    _log.Warn("There was a post to the SubmitManageProfileForm function but we cuold not find the member associated with this post");
                    ModelState.AddModelError("", "We could not find a user with that id to update");
                    return CurrentUmbracoPage();
                }
                //update email if email is different
                if (model.Member.Email != member.Email)
                {
                    //check if the new email already exists in the system
                    if (memberService.GetByEmail(model.Member.Email) != null)
                    {
                        ModelState.AddModelError("Email", "This email address already exists in the system and emails must be unique.");
                        return CurrentUmbracoPage();
                    }
                }
                try
                {
                    DataManager dm = new DataManager();
                    dm.AddOrUpdateMember(model.Member);
                    //mailing list subscribe
                    if (model.IsSubscribedToMailingList)
                        Task.Run(
                            () =>
                                dm.AddOrUpdateUserToMailChimpList(MailChimpListIds.MailingList, model.Member.Email,
                                    model.Member.FirstName, model.Member.LastName));
                    else
                    {
                        var mailingListMember = Task.Run(() => dm.IsUserSubscribedToMailChimpList(MailChimpListIds.MailingList, model.Member.Email)).Result;
                        if (mailingListMember)
                        {
                            Task.Run(
                            () =>
                                dm.AddOrUpdateUserToMailChimpList(MailChimpListIds.MailingList, model.Member.Email,
                                    model.Member.FirstName, model.Member.LastName,true));
                        }
                    }

                    //members mailing list subscribe
                    if (model.IsSubscribedToMailingList)
                        Task.Run(
                            () =>
                                dm.AddOrUpdateUserToMailChimpList(MailChimpListIds.Membership, model.Member.Email,
                                    model.Member.FirstName, model.Member.LastName));
                    else
                    {
                        var memberListMember = Task.Run(() => dm.IsUserSubscribedToMailChimpList(MailChimpListIds.Membership, model.Member.Email)).Result;
                        if (memberListMember)
                        {
                            Task.Run(
                            () =>
                                dm.AddOrUpdateUserToMailChimpList(MailChimpListIds.Membership, model.Member.Email,
                                    model.Member.FirstName, model.Member.LastName, true));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("There was an error updating a profile with the exception as follows",ex);
                    ModelState.AddModelError("", "There was an error updating your profile. Give us a shout if this keeps happening and we will find out whats going on");

                }
                if (ModelState.IsValid)
                {
                    TempData["Message"] = "You have successfully updated your profile. Woohooo!";
                    return RedirectToCurrentUmbracoPage();
                }
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
                var ordPlan = planService.Get(StaticIdentifiers.OrdinaryMemberPlanId);
                var socialPlan = planService.Get(StaticIdentifiers.SocialMemberPlanId);
                var patronPlan = planService.Get(StaticIdentifiers.PatronPlanId);
                if (ordPlan != null)
                {
                    model.OrdinaryPrice = (ordPlan.Amount / 100m).ToString("N");
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
                _log.Error($"There was a stripe error whilst trying to load the current subscriptions prices for the manage membership page. The error was {e.Message}.");
            }
            try
            {
                var stripeAccountId = new Member(Members.GetCurrentMember()).StripeUserId;
                var dm = new DataManager();
                if (stripeAccountId.IsNotNullOrEmpty())
                {
                    model.IsStripeUser = true;
                    model.HasExistingSubscription = false;
                    //Get plan status
                    var subscriptionService = new StripeSubscriptionService(SensativeInformation.StripeKeys.SecretKey);
                    var listOption = new StripeSubscriptionListOptions { CustomerId = stripeAccountId };
                    var stripeSubscriptions = subscriptionService.List(listOption);

                    if (stripeSubscriptions != null && stripeSubscriptions.Any())
                    {
                        model.IsOrdinaryMember = stripeSubscriptions.Any(m => m.StripePlan.Id == StaticIdentifiers.OrdinaryMemberPlanId && m.Status == StripeSubscriptionStatuses.Active);
                        model.IsSocialMember = stripeSubscriptions.Any(m => m.StripePlan.Id == StaticIdentifiers.SocialMemberPlanId && m.Status == StripeSubscriptionStatuses.Active);
                        model.IsPatron = stripeSubscriptions.Any(m => m.StripePlan.Id == StaticIdentifiers.PatronPlanId && m.Status == StripeSubscriptionStatuses.Active);
                        model.HasExistingSubscription = stripeSubscriptions.Any(m =>m.Status == StripeSubscriptionStatuses.Active);
                    }
                }
            }
            catch (StripeException e)
            {
                _log.Error($"There was a stripe error whilst trying to load the current subscriptions for the managemembership page for user {Members.GetCurrentMember().Name} and id {Members.GetCurrentMember().Id}. The error was {e.Message}.");
            }

            return PartialView("ManageSubscription", model);

        }


        public ActionResult MembershipHistory()
        {
            var dm = new BusinessLogic.DataManager();
            IEnumerable<Membership> model = dm.GetMembershipsForUser(Members.GetCurrentMemberId());
            return PartialView("MembershipHistory", model);
        }

        [HttpPost]
        public ActionResult SubmitStripeSubscriptionForm(StripeSubscriptionCheckout model)
        {
            if (ModelState.IsValid)
            {

                var loggedOnMember = Members.GetCurrentMember();
                var memberService = Services.MemberService;
                var member = memberService.GetById(loggedOnMember.Id);

                var customer = new StripeCustomerCreateOptions();
                customer.Email = member.Email;
                customer.Description = member.Name;

                customer.SourceToken = model.StripeToken;
                customer.PlanId = model.PlanId;
                try
                {
                    var customerService = new StripeCustomerService(SensativeInformation.StripeKeys.SecretKey);
                    StripeCustomer stripeCustomer = customerService.Create(customer);

                    //Log customer id on member
                    member.SetValue("stripeUserId", stripeCustomer.Id);
                    memberService.Save(member);
                    return RedirectToCurrentUmbracoPage();
                }
                catch (StripeException e)
                {
                    _log.Error(e.StripeError.Message);
                    ModelState.AddModelError("",
                        "There was an error setting up your subscription. Please try again. If the issue persists please contact us");
                }
            }
            return CurrentUmbracoPage();
        }
        [HttpPost]
        public ActionResult SubmitSubscribeToStripeSubscriptionForm(StripeSubscriptionCheckout model)
        {
            var loggedOnMember = Members.GetCurrentMember();
            var memberService = Services.MemberService;
            var member = memberService.GetById(loggedOnMember.Id);

            var stripeUserId = member.Properties.Contains("stripeUserId")
                ? member.Properties["stripeUserId"].Value as string
                : null;
            try
            {
                //Add subscription
                var subscriptionService = new StripeSubscriptionService(SensativeInformation.StripeKeys.SecretKey);
                var listOptions = new StripeSubscriptionListOptions { CustomerId = stripeUserId };
                var stripeSubscriptions = subscriptionService.List(listOptions);
                var update = stripeSubscriptions.Any(m => m.Status == StripeSubscriptionStatuses.Active);

                //if existingsubscripton update else create a new subscription
                if (update)
                {
                    var subscription =
                        stripeSubscriptions.FirstOrDefault(m => m.Status == StripeSubscriptionStatuses.Active);
                    if (subscription != null)
                    {
                        StripeSubscriptionUpdateOptions so = new StripeSubscriptionUpdateOptions { PlanId = model.PlanId };
                        so.PlanId = model.PlanId;

                        subscriptionService.Update(subscription.Id, so);
                        TempData["SuccessMessage"] =
                            "Congratulations! You have subscribed successfully. No more worrying about subs :)";
                        return RedirectToCurrentUmbracoPage();
                    }
                    else
                    {
                        _log.Error(
                            $"Tried to update a stripe subsciption for user with id {member.Id} but could not find any stripe subscriptions for this user.");
                        ModelState.AddModelError("",
                            "There was an error upgrading your subscription. Please try again. If the issue persists please contact us");
                        return CurrentUmbracoPage();
                    }
                }
                else
                {
                    StripeSubscription stripeSubscription = subscriptionService.Create(stripeUserId, model.PlanId);
                    TempData["SuccessMessage"] =
                            "Congratulations! You have subscribed successfully. No more worrying about subs :)";
                    return RedirectToCurrentUmbracoPage();
                }
            }
            catch (StripeException e)
            {
                _log.Error(e.StripeError.Message);
                ModelState.AddModelError("",
                    "There was an error setting up your subscription. Please try again. If the issue persists please contact us");
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult CancelSubscription(string subscriptionId)
        {
            if (subscriptionId.IsNullOrEmpty())
            {
                _log.Error("Canceling the subscription but the subscriptionId passed in was null or empty");
                ModelState.AddModelError("",
                    "There was an error whilst trying to cancel your subscription. Please try again or contact us so we can help you resolve this.");
                return CurrentUmbracoPage();
            }
            var loggedOnMember = Members.GetCurrentMember();


            var member = new Member(Umbraco.TypedMember(loggedOnMember.Id));

            if (member.StripeUserId.IsNotNullOrEmpty())
            {
                try
                {
                    var subscriptionService = new StripeSubscriptionService(SensativeInformation.StripeKeys.SecretKey);
                    StripeSubscription stripeSubscription = subscriptionService.Cancel(subscriptionId, true);
                    TempData["CancelMessage"] =
                        "You have successfully canceled your membership subscription. It can take five minutes to process.";
                    return RedirectToCurrentUmbracoPage();
                }
                catch (StripeException e)
                {
                    _log.Error(e.StripeError.Message);
                }
            }
            else
            {
                _log.Error($"The retrieved user had a null or empty stripe user id. The username is {member.Name}");
            }
            ModelState.AddModelError("",
                        "There was an error cancelling your subscription. Please try again. If the issue persists please contact us");
            return CurrentUmbracoPage();
        }

        public ActionResult ManageCards()
        {
            var loggedOnMember = Members.GetCurrentMember();
            var member = new Member(Umbraco.TypedMember(loggedOnMember.Id));
            IEnumerable<StripeCard> cardList = null;
            if (member.StripeUserId.IsNotNullOrEmpty())
            {
                StripeCardService cardService = new StripeCardService(SensativeInformation.StripeKeys.SecretKey);
                cardList = cardService.List(member.StripeUserId);
            }

            var model = cardList;

            return PartialView("ManageCards", model);
        }

        [HttpPost]
        public ActionResult UpdateCardDetails(string stripeToken)
        {
            if (stripeToken.IsNullOrEmpty())
            {
                _log.Error($"Tried to update a card but the supplied stripeToken was null or empty");
                ModelState.AddModelError("","There was an error whilst adding your new card details. Please try again. If the issue persists please contact us");
                return CurrentUmbracoPage();
            }

            var member = new Member(Members.GetCurrentMember());
            if (member.StripeUserId.IsNullOrEmpty())
            {
                _log.Error($"Somone tried to add a card when they did not have a stripe account");
                ModelState.AddModelError("", "You account is not currently capable of adding cards you must first signup for a membership subscription. If you already have done this please contact us to resolve this issue.");
                return CurrentUmbracoPage();
            }
            try
            {
                var custOptions = new StripeCustomerUpdateOptions();
                custOptions.SourceToken = stripeToken;

                var custService = new StripeCustomerService(SensativeInformation.StripeKeys.SecretKey);
                custService.Update(member.StripeUserId, custOptions);
                TempData["SuccessMessage"] = "Congrats you have successfully updated your card details!";
                return RedirectToCurrentUmbracoPage();
            }
            catch (StripeException e)
            {
                _log.Error(e.StripeError.Message);
                ModelState.AddModelError("",
                    "There was an error trying to update your card details. Please contact us if this problem persists.");
            }
            return CurrentUmbracoPage();
        }
    }
}