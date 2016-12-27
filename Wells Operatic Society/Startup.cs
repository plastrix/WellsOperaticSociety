using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using Umbraco.Web;
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

            RecurringJob.AddOrUpdate(() => WellsOperaticSociety.Web.ScheduledTasks.HangfireScheduledTasks.MembershipRenewalReminders(), Cron.Daily());
        }
    }
}