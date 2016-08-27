using System.Web.Optimization;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using WellsOperaticSociety.BusinessLogic;

namespace WellsOperaticSociety.Web
{
    public class UmbracoStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ContentService.Published += ContentService_Published;

            base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        private void ContentService_Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            DataManager manager = new DataManager(Umbraco.Web.UmbracoContext.Current);
            manager.PublishRobots();
            manager.PublishSitemap();
        }
    }
}