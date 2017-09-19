using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using System.Web;
using Umbraco.Web;
using WellsOperaticSociety.Web.HangFire;

[assembly: OwinStartup(typeof(WellsOperaticSociety.Web.Startup))]
namespace WellsOperaticSociety.Web
{
    public class Startup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            base.Configuration(app);
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new AuthorizationFilter() }
                //AuthorizationFilters = new[] { new AuthorizationFilter { Roles = "Committee Member" } };
            });
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate(() => HangfireScheduledTasks.MembershipRenewalReminders(), Cron.Daily());
            RecurringJob.AddOrUpdate(() => HangfireScheduledTasks.RenewLifeMembers(), Cron.Daily());
        }
        private class AuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = HttpContext.Current;

                if (httpContext == null || httpContext.User == null) return false;

                return httpContext.User.IsInRole("Committee Member");
            }
        }
    }
}