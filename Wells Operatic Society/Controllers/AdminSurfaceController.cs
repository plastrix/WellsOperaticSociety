using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using log4net;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Stripe;
using Umbraco.Web.Mvc;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.EmailService;
using WellsOperaticSociety.Models;
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.EmailModels;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.Models.ReportModels;
using WellsOperaticSociety.Web.Helper;
using WellsOperaticSociety.Web.Models;
using Member = WellsOperaticSociety.Models.MemberModels.Member;

namespace WellsOperaticSociety.Web.Controllers
{
    public class AdminSurfaceController : SurfaceController
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AdminSurfaceController()
        {
            this.ActionInvoker = new RenderActionInvokerAsync();
        }

        public ActionResult ManageMembers(int? pageSize, int? row)
        {
            var model = new ManageMembershipViewModel
            {
                Members = Services.MemberService.GetAllMembers().Take(50)
            };

            return PartialView("ManageMembership",model);
        }

        public ActionResult ManageMember(int id)
        {

            DataManager dm = new DataManager();
            var member = Members.GetById(id);
            if (member == null)
            {
                TempData["ErrorMessage"] =
                    "The member you are looking for could not be found. Please try again but if it persists please give your handy IT people a shout :)";
                _log.Error($"A member with the id of {id} could not be found in the admin surface controller ManageMember function");
                return null;
            }

            var model = new ManageMemberViewModel();
            model.Member = new Member(member);
            model.Memberships = dm.GetMembershipsForUser(id);

            //setup new membership
            var curMemberships = dm.GetMembershipsForUser(id);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddYears(1);
            if (curMemberships !=null && curMemberships.Count>0)
            {
                var previous = curMemberships.OrderByDescending(m => m.EndDate).First();
                startDate = previous.StartDate.AddYears(1);
                endDate = startDate.AddYears(1).AddDays(-1);
            }

            model.NewMembership = new Membership()
            {
                StartDate = startDate,
                EndDate = endDate,
                Member = id,
                IsSubscription = false
            };

            try
            {
                model.IsSubscribedToMailingList = Task.Run(() => dm.IsUserSubscribedToMailChimpList(MailChimpListIds.MailingList, model.Member.Email)).Result;
                model.IsSubscribedToMemberList = Task.Run(() => dm.IsUserSubscribedToMailChimpList(MailChimpListIds.Membership, model.Member.Email)).Result;
            }
            catch (Exception ex)
            {
                _log.Error("Error fetching mailchimp subscription information", ex);
            }

            return PartialView("ManageMember",model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMember(ManageMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                //No user id was passed in
                if (model.Member.Id == 0)
                {
                    _log.Error("Trying to update a member on the admin form but no userId was posted back");
                    ModelState.AddModelError("","We could not find a user with that id to update");
                    return CurrentUmbracoPage();
                }

                var memberService = Services.MemberService;
                var member = memberService.GetById(model.Member.Id);
                //couldnt find member
                if (member == null)
                {
                    _log.Error($"Trying to update a member on the admin form but the member with id {model.Member.Id} could not be found");
                    ModelState.AddModelError("", "We could not find a user with that id to update");
                    return CurrentUmbracoPage();
                }
                //update email if email is different
                if (model.Member.Email != member.Email)
                {
                    //check if the new email already exists in the system
                    if (memberService.GetByEmail(model.Member.Email) != null)
                    {
                        ModelState.AddModelError("Email","This email address already exists in the system and emails must be unique.");
                        return CurrentUmbracoPage();
                    }
                }
                
                try
                {
                    DataManager dm = new DataManager();
                    dm.AddOrUpdateMember(model.Member,true);
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
                                    model.Member.FirstName, model.Member.LastName, true));
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
                    _log.Error("There was an error updating a profile with the exception as follows", ex);
                    ModelState.AddModelError("", "There was an error updating this profile. Give us a shout if this keeps happening and we will find out whats going on");
                    return CurrentUmbracoPage();
                }
                TempData["SuccessMessage"] =
                    $"Hurray!! Everything worked and you have update the profile for {model.Member.FirstName} {model.Member.LastName}";
                return RedirectToCurrentUmbracoPage("?id=" +model.Member.Id);
            }
            return CurrentUmbracoPage();
        }

        public PartialViewResult MemberSearch(string search)
        {
            var model = Services.MemberService.GetAllMembers().Where(m=>m.Email.ToLower().Contains(search.ToLower()) || m.Name.ToLower().Contains(search.ToLower()));
            return PartialView("ManageMemberList",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMembership(Membership membership)
        {
            if (ModelState.IsValid)
            {
                var dm = new DataManager();
                dm.AddOrUpdateMembership(membership);
                TempData["SuccessMessage"] = "You have successfully added a membership record to this user";
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult DeleteMembership()
        {
            int membershipId;
            
            if (!int.TryParse(Request.Form["membership.MembershipId"], out membershipId))
            {
                TempData["ErrorMessage"] =
                    "There was an error whilst deleting the membership line. Try again or contact the system else who might be in the know.... not Nick though as he is probably busy!";
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            var dm = new DataManager();
            dm.DeleteMembership(membershipId);
            TempData["SuccessMessage"] =
                    $"You have successfully deleted the membership with id {membershipId} and removed them from the mailchimp Members list if they are not an active member any more";
            return RedirectToCurrentUmbracoPage(Request.QueryString);
        }

        [HttpPost]
        public ActionResult CancelSubscription()
        {
            int memberId;
            if (!int.TryParse(Request.Form["membership.Member"], out memberId))
            {
                TempData["ErrorMessage"] =
                   "There was an error whilst canceling the subscription. Try again or contact the system else who might be in the know.... not Nick though as he is probably busy!";

                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            if (Request.Form["membership.StripeSubscriptionId"]==null)
            {
                TempData["ErrorMessage"] =
                   "There was an error whilst canceling the subscription. Try again or contact the system else who might be in the know.... not Nick though as he is probably busy!";

                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            string subscriptionId = Request.Form["membership.StripeSubscriptionId"];
            var m = new Member(Members.GetById(memberId));
            if (m.StripeUserId.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] =
                   "There was an error whilst canceling the subscription. Try again or contact the system else who might be in the know.... not Nick though as he is probably busy!";

                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            try
            {
                StripeSubscriptionService service =
                    new StripeSubscriptionService(SensativeInformation.StripeKeys.SecretKey);
                service.Cancel(m.StripeUserId, subscriptionId, true);
            }
            catch (StripeException e)
            {
                _log.Error($"Admin tried to cancel subscription but stripe returned the error: {e.Message}");
                TempData["ErrorMessage"] =
                   $"There was an error whilst canceling the subscription. The service retrurned the error {e.Message}";

                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            TempData["SuccessMessage"] =
                    $"You have successfully deleted the subsctipn with id {subscriptionId}. This can take a few seconds to take effect.";
            return RedirectToCurrentUmbracoPage(Request.QueryString);
        }

        public ActionResult AddMembersToFunction(int functionId)
        {
            var dm = new DataManager();
            
            var model = new AddMembersToFunctionViewModel();
            model.Function = dm.GetFunction(functionId);
            model.MemberRolesInShows = dm.GetMemberRolesInFunction(functionId);
            model.NewMemberRolesInShow = new MemberRolesInShow() {FunctionId = functionId};
            model.MostUsedGroups = dm.GetMostUsedRoles(10);
            if (TempData["LastGroup"] != null)
                model.NewMemberRolesInShow.Group = TempData["LastGroup"].ToString();

            return PartialView("AddMembersToFunction",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMemberToFunction(MemberRolesInShow model)
        {
            if (ModelState.IsValid)
            {
                var dm = new DataManager();
                dm.CreateMemberInFunction(model);
                TempData["LastGroup"] = model.Group;
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult RemoveMemberFromFunction(int memberRolesInShowId)
        {
            var dm = new DataManager();
            dm.DeleteMemberRoleInFunction(memberRolesInShowId);
            TempData["SuccessMessage"] =
                $"You have successfully managed to remove that role and person.... but I am too lazy to say who they were in the message. So just know I think you are doing a grand job!";
            return RedirectToCurrentUmbracoPage(Request.QueryString);
        }

        public ActionResult GetRoleSuggestions(string term)
        {
            var dm = new DataManager();
            var results = dm.GetRoleSuggestions(term);
            return Json(results,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGroupSuggestions(string term)
        {
            var dm = new DataManager();
            var results = dm.GetGroupSuggestions(term);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMemberSuggestions(string term)
        {
            var dm = new DataManager();
            var result = dm.AcitveMemberSuggestions(term);
            var jsonStr = JsonConvert.SerializeObject(result);
            return Content(jsonStr, "application/json");
            //return Json(jsonStr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageSeats()
        {
            var dm = new DataManager();
            var model = new ManageSeatsViewModel();
            model.Seats = dm.GetSeats();
            model.NewSeat = new Seat();
            return PartialView("ManageSeats", model);
        }
        [HttpPost]
        public ActionResult DeleteSeat(int seatId)
        {
            if (ModelState.IsValid)
            {
                var dm = new DataManager();
                dm.DeleteSeat(seatId);
                TempData["SuccessMessage"] = $"You have managed to remove that seat.... I hope you are having fun :)";
                return RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSeat(Seat seat)
        {
            if (ModelState.IsValid)
            {
                var dm = new DataManager();
                seat.SeatNumber = seat.SeatNumber.ToUpper();
                dm.AddSeat(seat);
                TempData["SuccessMessage"] = $"You have added seat {seat.SeatNumber}. Time to open the Champaign.";
                return RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }

        [HttpGet]
        public FileStreamResult VehicleRegistrationReport()
        {
            var dm = new DataManager();
            var pdfService = new PDFService.PdfWriter();
            var model = new VehicleRgeistrationViewModel();
            model.BaseUri = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            model.RegistrationList = dm.GetVehicleRegistrations();
            ViewData.Model = model;
            var html = RazorHelpers.RenderRazorViewToString("~/Views/Reports/VehicleRegistrationReport.cshtml",
                ControllerContext, ViewData, TempData);
            var stream = pdfService.GenertatePdfFromHtml(html);

            return File(stream, "application/pdf");
        }

        public ActionResult ManageLongServiceAwards()
        {
            var dm = new DataManager();
            var model = new ManageLongServiceReportViewModel();
            model.DueAwards = dm.GetDueLongServiceAwards();
            model.AlreadyPresentedAwards = dm.GetAwardedLongServiceAwards();

            return PartialView("ManageLongServiceAwards",model);
        }

        [HttpPost]
        public ActionResult SubmitLongServiceAward(LongServiceAward longServiceAward)
        {
            if (ModelState.IsValid)
            {
                var dm = new DataManager();
                dm.AddOrUpdateLongServiceAward(longServiceAward);
                TempData["SuccessMessage"] =
                    $"You have awarded/hidden/revealed the {longServiceAward.Award} successfully. Jolly good show!";
                return RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();

        }

        [HttpGet]
        public FileStreamResult LongServiceReport()
        {
            var dm = new DataManager();
            var pdfService = new PDFService.PdfWriter();
            var model = new LongServiceModel();
            var baseUri = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            model.BaseUri = baseUri;
            model.DueAwards = dm.GetDueLongServiceAwards().Where(m=>m.Hide == false).ToList();
            ViewData.Model = model;
            var html = RazorHelpers.RenderRazorViewToString("~/Views/Reports/LongServiceReport.cshtml",
                ControllerContext, ViewData, TempData);
            var stream = pdfService.GenertatePdfFromHtml(html);

            return File(stream, "application/pdf");
        }

        public ActionResult ManageVouchers(int id)
        {
            ShowVouchersViewModel model;

            if (id != 0)
            {
                var dm = new DataManager();
                model = new ShowVouchersViewModel
                {
                    FunctionId = id,
                    MemberVoucherList = dm.GetVouchersForShow(id, VoucherMember.Member),
                    PatronVoucherList = dm.GetVouchersForShow(id, VoucherMember.Patron),
                    ShowMemberVoucherList = dm.GetVouchersForShow(id, VoucherMember.ShowMember),
                    BoxOfficeOpenDate = DateTime.Now
                };
                model.MembersCount = model.MemberVoucherList.Count();
                model.PatronsCount = model.PatronVoucherList.Count();
                model.ShowMembersCount = model.ShowMemberVoucherList.Count();
            }
            else
            {
                model = new ShowVouchersViewModel();
            }

            return PartialView("ManageVouchers", model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SendVoucher(Voucher voucher,ste)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/ShowVouchers.cshtml", ControllerContext,
        //            ViewData, TempData);
        //        EmailService.EmailHelpers emailsService = new EmailHelpers();
        //        emailsService.SendEmail(mem.ShowMemberVoucherList[i].Member.GetContactEmail,
        //            $"Pre booking for {mem.ShowMemberVoucherList[i].Function.DisplayName}", html);
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendVouchers(ShowVouchersModel model)
        {
            if (ModelState.IsValid)
            {
                var dm = new DataManager();

                if (model.ShowMemberVouchers.IsNotNullOrEmpty())
                {
                    var showMemberVouchers = dm.GetVouchersForShow(model.FunctionId, VoucherMember.ShowMember);
                    var vouchers = model.ShowMemberVouchers.Split(',').ToList();
                    if (vouchers.Count < showMemberVouchers.Count)
                        ModelState.AddModelError("ShowMemberVouchers",
                            $"You need at least {showMemberVouchers.Count} vouchers for the show member vouchers and you only suplied {vouchers.Count}");
                    else
                    {
                        for (var i = 0; i < showMemberVouchers.Count; i++)
                        {
                            showMemberVouchers[i].Key = vouchers[i];
                            //save
                            dm.AddOrUpdateShowVoucher(vouchers[i], showMemberVouchers[i].MemberId, model.FunctionId);
                            //Email vouchers
                            dm.SendVoucherEmail(showMemberVouchers[i], model.BoxOfficeOpenDate.ToString("dd/MM/yyyy"), model.BookingUrl,
                                ControllerContext, ViewData, TempData);
                        }
                    }
                }

                if (model.PatronVouchers.IsNotNullOrEmpty())
                {
                    var patronVouchers = dm.GetVouchersForShow(model.FunctionId, VoucherMember.Patron);
                    var vouchers = model.PatronVouchers.Split(',').ToList();
                    if (vouchers.Count < patronVouchers.Count)
                        ModelState.AddModelError("PatronVouchers",
                            $"You need at least {patronVouchers.Count} vouchers for the show member vouchers and you only suplied {vouchers.Count}");
                    else
                    {
                        for (var i = 0; i < patronVouchers.Count; i++)
                        {
                            patronVouchers[i].Key = vouchers[i];
                            //save
                            dm.AddOrUpdateShowVoucher(vouchers[i], patronVouchers[i].MemberId, model.FunctionId);
                            //Email vouchers
                            dm.SendVoucherEmail(patronVouchers[i], model.BoxOfficeOpenDate.ToString("dd/MM/yyyy"), model.BookingUrl,
                                ControllerContext, ViewData, TempData);
                        }
                    }
                }

                if (model.MemberVouchers.IsNotNullOrEmpty())
                {
                    var memberVouchers = dm.GetVouchersForShow(model.FunctionId, VoucherMember.Member);
                    var vouchers = model.MemberVouchers.Split(',').ToList();
                    if (vouchers.Count < memberVouchers.Count)
                        ModelState.AddModelError("MemberVouchers",
                            $"You need at least {memberVouchers.Count} vouchers for the show member vouchers and you only suplied {vouchers.Count}");
                    else
                    {
                        for (var i = 0; i < memberVouchers.Count; i++)
                        {
                            memberVouchers[i].Key = vouchers[i];
                            //save
                            dm.AddOrUpdateShowVoucher(vouchers[i], memberVouchers[i].MemberId, model.FunctionId);
                            //Email vouchers
                            dm.SendVoucherEmail(memberVouchers[i], model.BoxOfficeOpenDate.ToString("dd/MM/yyyy"), model.BookingUrl,
                                ControllerContext, ViewData, TempData);
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    TempData["Success"] = "The vouchers are winging their way to our extremely lovely membership. Well done!";
                }
                return RedirectToCurrentUmbracoPage($"id={model.FunctionId}");
            }
            return CurrentUmbracoPage();
        }
    }
}