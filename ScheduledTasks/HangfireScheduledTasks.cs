using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WellsOperaticSociety.Models.EmailModels;

namespace WellsOperaticSociety.BusinessLogic
{
    public static class HangfireScheduledTasks
    {
        public static void MembershipRenewalReminders()
        {
            HttpContextWrapper c = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", typeof(MembershipSurfaceController).Name.ToLower().Replace("controller", ""));
            ControllerContext controllerContext = new ControllerContext(new RequestContext(c, routeData), new MembershipSurfaceController());
            ViewDataDictionary viewData = new ViewDataDictionary();
            TempDataDictionary tempData = new TempDataDictionary();


            DataManager dm = new DataManager();
            var members = dm.GetCurrentMemberships();
            //send reminder a week before

            var model = new MembershipRenewal();
            model.BaseUri = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            ViewData.Model = model;
            var html = RazorHelpers.RenderRazorViewToString("~/Views/Reports/VehicleRegistrationReport.cshtml",
                ControllerContext, ViewData, TempData);
            var stream = pdfService.GenertatePdfFromHtml(html);


            foreach (var m in members.Where(n => n.EndDate.Date >= DateTime.UtcNow.Date.AddDays(-100)).ToList())
            {
                if(m.IsSubscription)
                    html = RazorHelpers.RenderRazorViewToString("")

            }

            foreach (var m in members.Where(n => n.EndDate.Date >= DateTime.UtcNow.Date).ToList())
            {
                Debug.WriteLine($"Membership a week from renewal. Member id: {m.Member} End date: {m.EndDate} MembershipId: {m.MembershipId}");
            }


        }
    }
}
