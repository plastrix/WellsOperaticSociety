using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Umbraco.Web.Mvc;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.Web.Models;
using Member = WellsOperaticSociety.Models.MemberModels.Member;

namespace WellsOperaticSociety.Web.Controllers
{
    public class AdminSurfaceController : SurfaceController
    {
        public ActionResult ManageMembership()
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
                member.SetValue("disabled", model.Member.Disabled);
                memberService.Save(member);
                //TODO: Display success message
                return RedirectToCurrentUmbracoPage("?id=" +model.Member.Id);
            }
            return CurrentUmbracoPage();
        }
    }
}