using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Umbraco.Web.Mvc;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.Models.ReportModels;
using WellsOperaticSociety.Web.Models;
using Member = WellsOperaticSociety.Models.MemberModels.Member;

namespace WellsOperaticSociety.Web.Controllers
{
    public class AdminSurfaceController : SurfaceController
    {
        public ActionResult ManageMembers()
        {
            var model = new ManageMembershipViewModel();
            model.Members = Services.MemberService.GetAllMembers();
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
                endDate = startDate.AddYears(1);
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
                memberService.Save(member);
                //TODO: Display success message
                return RedirectToCurrentUmbracoPage("?id=" +model.Member.Id);
            }
            return CurrentUmbracoPage();
        }

        public PartialViewResult MemberSearch(string search)
        {
            var model = Services.MemberService.GetAllMembers().Where(m=>m.Email.Contains(search) || m.Username.Contains(search));
            return PartialView("ManageMemberList",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMembership(Membership membership)
        {
            if (ModelState.IsValid)
            {
                var dm = new DataManager();
                dm.CreateMembership(membership);
                //TODO: Add success page
                RedirectToCurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult DeleteMembership()
        {
            int membershipId;
            
            if (!int.TryParse(Request.Form["membership.MembershipId"], out membershipId))
            {
                //TODO:Error Message like succes but error
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            var dm = new DataManager();
            dm.DeleteMembership(membershipId);
            //TODO:Success message
            return RedirectToCurrentUmbracoPage(Request.QueryString);
        }

        public ActionResult AddMembersToFunction(int functionId)
        {
            var dm = new DataManager();
            
            var model = new AddMembersToFunctionViewModel();
            model.Function = dm.GetFunction(functionId);
            model.MemberRolesInShows = dm.GetMemberRolesInFunction(functionId);
            model.NewMemberRolesInShow = new MemberRolesInShow() {FunctionId = functionId};
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
            var model = dm.GetVehicleRegistrations();
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
            model.DueAwards = dm.GetDueLongServiceAwards().Where(m=>m.Hide == false).ToList();
            ViewData.Model = model;
            var html = RazorHelpers.RenderRazorViewToString("~/Views/Reports/LongServiceReport.cshtml",
                ControllerContext, ViewData, TempData);
            var stream = pdfService.GenertatePdfFromHtml(html);

            return File(stream, "application/pdf");
        }
    }
}