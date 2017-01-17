using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
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

            app.UseHangfireDashboard("/hangfire",new DashboardOptions()
            {
                AuthorizationFilters = new [] {new AuthorizationFilter {Roles = "Committee Member" } }
            });
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate(() => HangfireScheduledTasks.MembershipRenewalReminders(), Cron.Daily());
            RecurringJob.AddOrUpdate(() => HangfireScheduledTasks.RenewLifeMembers(), Cron.Daily());
        }
    }
}