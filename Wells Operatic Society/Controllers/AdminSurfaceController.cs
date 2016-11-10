﻿using System;
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
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.EmailModels;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.Models.ReportModels;
using WellsOperaticSociety.Web.Models;
using Member = WellsOperaticSociety.Models.MemberModels.Member;

namespace WellsOperaticSociety.Web.Controllers
{
    public class AdminSurfaceController : SurfaceController
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
                //TODO: report error to user
                //TODO: log error
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
                    //TODO:LogError
                    ModelState.AddModelError("","We could not find a user with that id to update");
                    return CurrentUmbracoPage();
                }

                var memberService = Services.MemberService;
                var member = memberService.GetById(model.Member.Id);
                //couldnt find member
                if (member == null)
                {
                    //TODO:LogError
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
                    member.Username = model.Member.Email;
                    member.Email = model.Member.Email;
                }

                member.Name = model.Member.Name;
                member.SetValue("telephoneNumber",model.Member.TelephoneNumber);
                member.SetValue("mobileNumber", model.Member.MobileNumber);
                member.SetValue("dateOfBirth", model.Member.DateOfBirth);
                member.SetValue("dateAppliedForMembership", model.Member.DateAppliedForMembership);
                member.SetValue("dateApprovedForMembership", model.Member.DateApprovedForMembership);
                member.SetValue("dateDeclinedForMembership", model.Member.DateDeclinedForMembership);
                member.SetValue("dateLifeMembershipGranted", model.Member.DateLifeMembershipGranted);
                member.SetValue("stripeUserId", model.Member.StripeUserId);
                member.SetValue("vehicleRegistration1", model.Member.VehicleRegistration1);
                member.SetValue("vehicleRegistration2", model.Member.VehicleRegistration2);
                member.SetValue("deactivated", model.Member.Deactivated);
                member.SetValue("contactEmail", model.Member.ContactEmail);
                memberService.Save(member);
                //TODO: Display success message
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
        public async Task<ActionResult> AddMembership(Membership membership)
        {
            if (ModelState.IsValid)
            {
                var dm = new DataManager();
                await dm.AddOrUpdateMembership(membership);
                //TODO: Add success page
                RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteMembership()
        {
            int membershipId;
            
            if (!int.TryParse(Request.Form["membership.MembershipId"], out membershipId))
            {
                TempData["ErrorMessage"] =
                    "There was an error whilst deleting the membership line. Try again or contact the system else who might be in the know.... not Nick though as he is probably busy!";
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            var dm = new DataManager();
            await dm.DeleteMembership(membershipId);
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
            //TODO:Validation is not working
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
            //TODO:Success message
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
                //TODO:Success message
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
                //TODO:Success message
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
            }
            //TODO: display any errors
            return RedirectToCurrentUmbracoPage();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendVouchers(int functionId, string showMemberVouchers, string membersVouchers, string patronsVouchers, string voucherActiveFrom,string showBoxOfficeUrl)
        {
            var dm = new DataManager();
            var mem = dm.GetMemberShowVouchers(functionId);

            if (showMemberVouchers.IsNotNullOrEmpty())
            {
                var vouchers = showMemberVouchers.Split(',').ToList();
                if (vouchers.Count < mem.ShowMemberVoucherList.Count)
                    ModelState.AddModelError("",
                        $"You need at least {mem.ShowMemberVoucherList.Count} vouchers for the show member vouchers and you only suplied {vouchers.Count}");
                else
                {
                    for(var i=0;i<mem.ShowMemberVoucherList.Count;i++)
                    {
                        //save
                        dm.AddOrUpdateShowVoucher(vouchers[i], mem.ShowMemberVoucherList[i].MemberId,functionId);
                        //Email vouchers
                        ShowVoucher model = new ShowVoucher();
                        model.Member = mem.ShowMemberVoucherList[i].Member;
                        model.Function = mem.ShowMemberVoucherList[i].Function;
                        model.DateActive = voucherActiveFrom;
                        model.Key = vouchers[i];
                        model.Link = showBoxOfficeUrl + "?v=" + vouchers[i];
                        model.BaseUri = UrlHelpers.GetBaseUrl();
                        ViewData.Model = model;
                        //send email
                        var html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/ShowVouchers.cshtml", ControllerContext,
                            ViewData, TempData);
                        EmailService.EmailHelpers emailsService = new EmailHelpers();
                        emailsService.SendEmail(mem.ShowMemberVoucherList[i].Member.GetContactEmail,
                            $"Pre booking for {mem.ShowMemberVoucherList[i].Function.Name}", html);
                    }
                }
            }

            if (membersVouchers.IsNotNullOrEmpty())
            {
                var vouchers = membersVouchers.Split(',').ToList();
                if (vouchers.Count < mem.MemberVoucherList.Count)
                    ModelState.AddModelError("",
                        $"You need at least {mem.MemberVoucherList.Count} vouchers for the show member vouchers and you only suplied {vouchers.Count}");
                else
                {
                    for (var i = 0; i < mem.MemberVoucherList.Count; i++)
                    {
                        //save
                        dm.AddOrUpdateShowVoucher(vouchers[i], mem.MemberVoucherList[i].MemberId, functionId);
                        //send email
                        ShowVoucher model = new ShowVoucher();
                        model.Member = mem.MemberVoucherList[i].Member;
                        model.Function = mem.MemberVoucherList[i].Function;
                        model.DateActive = voucherActiveFrom;
                        model.Key = vouchers[i];
                        model.Link = showBoxOfficeUrl + "?v=" + vouchers[i];
                        model.BaseUri = UrlHelpers.GetBaseUrl();
                        ViewData.Model = model;
                        //send email
                        var html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/ShowVouchers", ControllerContext,
                            ViewData, TempData);
                        EmailService.EmailHelpers emailsService = new EmailHelpers();
                        emailsService.SendEmail(mem.MemberVoucherList[i].Member.GetContactEmail,
                            $"Pre booking for {mem.MemberVoucherList[i].Function.Name}", html);
                    }
                }
            }

            if (patronsVouchers.IsNotNullOrEmpty())
            {
                var vouchers = patronsVouchers.Split(',').ToList();
                if (vouchers.Count < mem.PatronVoucherList.Count)
                    ModelState.AddModelError("",
                        $"You need at least {mem.PatronVoucherList.Count} vouchers for the show member vouchers and you only suplied {vouchers.Count}");
                else
                {
                    for (var i = 0; i < mem.PatronVoucherList.Count; i++)
                    {
                        //save
                        dm.AddOrUpdateShowVoucher(vouchers[i], mem.PatronVoucherList[i].MemberId, functionId);
                        //send email
                        ShowVoucher model = new ShowVoucher();
                        model.Member = mem.PatronVoucherList[i].Member;
                        model.Function = mem.PatronVoucherList[i].Function;
                        model.DateActive = voucherActiveFrom;
                        model.Key = vouchers[i];
                        model.Link = showBoxOfficeUrl + "?v=" + vouchers[i];
                        model.BaseUri = UrlHelpers.GetBaseUrl();
                        ViewData.Model = model;
                        //send email
                        var html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/ShowVouchers", ControllerContext,
                            ViewData, TempData);
                        EmailService.EmailHelpers emailsService = new EmailHelpers();
                        emailsService.SendEmail(mem.PatronVoucherList[i].Member.GetContactEmail,
                            $"Pre booking for {mem.PatronVoucherList[i].Function.Name}", html);
                    }
                }
            }
            return CurrentUmbracoPage();
        }
    }
}