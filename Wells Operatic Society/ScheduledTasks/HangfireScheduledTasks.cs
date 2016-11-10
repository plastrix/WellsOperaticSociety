using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.Models.EmailModels;
using WellsOperaticSociety.Web.Controllers;
using RazorEngine;
using RazorEngine.Templating;

namespace WellsOperaticSociety.Web.ScheduledTasks
{
    public static class HangfireScheduledTasks
    {
        public static void MembershipRenewalReminders()
        {
            DataManager dm = new DataManager();
            var members = dm.GetCurrentMemberships();


            var model = new MembershipRenewal();
            model.BaseUri = "https://www.wellslittletheatre.com";

            //TODO: change this to equals and to 7 days
            //send reminder a week before
            foreach (var m in members.Where(n => n.EndDate.Date >= DateTime.UtcNow.Date.AddDays(-100)).ToList())
            {
                model.MembershipType = m.MembershipTypeName;
                model.DateDue = m.EndDate.AddDays(1);
                var html = Engine.Razor.RunCompile("~/Views/Emails/MembershipRenewalCommingUp.cshtml","MembershipRenewal", typeof(MembershipRenewal),model);
            }

            foreach (var m in members.Where(n => n.EndDate.Date >= DateTime.UtcNow.Date).ToList())
            {
                Debug.WriteLine($"Membership a week from renewal. Member id: {m.Member} End date: {m.EndDate} MembershipId: {m.MembershipId}");
            }


        }
    }
}
