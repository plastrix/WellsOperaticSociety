using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using RazorEngine;
using RazorEngine.Templating;
using Stripe;
using Umbraco.Web;
using WellsOperaticSociety.BusinessLogic;
using WellsOperaticSociety.EmailService;
using WellsOperaticSociety.Models.EmailModels;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.MemberModels;
using Umbraco.Core;
using System.Web;
using System.Web.Hosting;
using Umbraco.Web.Security;
using Umbraco.Core.Configuration;
using Umbraco.Web.Routing;

namespace WellsOperaticSociety.Web.HangFire
{
    public static class HangfireScheduledTasks
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void MembershipRenewalReminders()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current ?? new HttpContext(new SimpleWorkerRequest("temp.aspx", "", new StringWriter())));

            var context = UmbracoContext.EnsureContext(httpContext,ApplicationContext.Current,new WebSecurity(httpContext, ApplicationContext.Current),UmbracoConfig.For.UmbracoSettings(),UrlProviderResolver.Current.Providers,false);

            DataManager dm = new DataManager(context);
            var members = dm.GetCurrentMemberships();


            var model = new MembershipRenewal();
            model.BaseUri = "https://www.wellslittletheatre.com";
            var subscriptionService = new StripeSubscriptionService(SensativeInformation.StripeKeys.SecretKey);
            string price = string.Empty;

            //send reminder a week before
            foreach (
                var m in
                    members.Where(
                        n =>
                            (n.IsSubscription == false || (n.IsSubscription && n.CancelAtEnd == false)) &&
                            n.EndDate.Date == DateTime.UtcNow.Date.AddMonths(-1) ||
                            n.EndDate.Date == DateTime.UtcNow.Date.AddDays(-7)).ToList())
            {
                var member = dm.GetMember(m.Member);
                if (member != null)
                {
                    if (m.IsSubscription && m.StripeSubscriptionId != null)
                    {
                        try
                        {
                            if (member.StripeUserId.IsNotNullOrEmpty())
                            {
                                var subscription = subscriptionService.Get(m.StripeSubscriptionId);
                                price = (subscription.StripePlan.Amount/100m).ToString("N");
                            }
                        }
                        catch
                            (StripeException e)
                        {
                            Log.Error(
                                $"There was a stripe error whilst trying to load the current subscription price for the membership{m.MembershipId}. The error was {e.Message}.");
                        }
                    }
                    model.MembershipType = m.MembershipTypeName;
                    model.DateDue = m.EndDate.AddDays(1);
                    model.IsSubscription = m.IsSubscription;
                    model.Price = price;
                    model.Name = member.Name;
                    var fileContents =
                        GetFileContents("~/MembershipRenwalEmailContent/MembershipRenewalCommingUp.cshtml");
                    var html = Engine.Razor.RunCompile(fileContents, "MembershipRenewal", typeof(MembershipRenewal),
                        model);

                    EmailHelpers emailService = new EmailHelpers();
                    emailService.SendEmail(member.GetContactEmail,"Membership renewal is comming up!",html);
                }
            }
        }

        public static void RenewLifeMembers()
        {

            var httpContext = new HttpContextWrapper(HttpContext.Current ?? new HttpContext(new SimpleWorkerRequest("temp.aspx", "", new StringWriter())));

            var context = UmbracoContext.EnsureContext(httpContext, ApplicationContext.Current, new WebSecurity(httpContext, ApplicationContext.Current), UmbracoConfig.For.UmbracoSettings(), UrlProviderResolver.Current.Providers, false);


            DataManager dm = new DataManager(context);
            var memberships = dm.GetCurrentMemberships();
            foreach (var m in memberships)
            {
                if (m.MembershipType == MembershipType.Life && m.EndDate.Date == DateTime.UtcNow.Date)
                {
                    //create new membership for user
                    var newMembership = new Membership();
                    newMembership.Member = m.Member;
                    newMembership.MembershipType = MembershipType.Life;
                    newMembership.IsSubscription = false;
                    newMembership.StartDate = m.StartDate.AddYears(1);
                    newMembership.EndDate = m.EndDate.AddYears(1);

                    dm.AddOrUpdateMembership(newMembership);

                }
            }
        }

        private static string GetFileContents(string filePath)
        {
            string mappedFilepath = System.Web.Hosting.HostingEnvironment.MapPath(filePath);

            try
            {
                if(mappedFilepath.IsNullOrEmpty())
                    return string.Empty;

                string content;
                using (var stream = new StreamReader(mappedFilepath))
                {
                    content = stream.ReadToEnd();
                }
                return content;
            }
            catch (Exception ex)
            {
                Log.Error($"Error trying to find email content. For the membership renewal upcoming email. Looked for {mappedFilepath}",ex);
                return string.Empty;
            }
        }
    }
}
