using System.Web.Optimization;

namespace WellsOperaticSociety.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/jqueryAndBootstrapScripts").Include(
                "~/scripts/jquery-{version}.js",
                "~/scripts/jquery-ui-1.12.0.js",
                "~/scripts/datepicker-en-GB.js",
                "~/scripts/bootstrap.js",
                "~/scripts/jquery.validate.min.js",
                "~/scripts/jquery.validate.unobtrusive.min.js",
                "~/scripts/jquery.unobtrusive-ajax.min.js",
                "~/scripts/chartist.min.js",
                "~/scripts/datatables/jquery.dataTables.js",
                "~/scripts/datatables/dataTables.bootstrap.js",
                "~/scripts/datatables/dataTables.buttons.js",
                "~/scripts/datatables/buttons.bootstrap.js",
                "~/scripts/datatables/buttons.flash.js",
                "~/scripts/datatables/buttons.html5.js"));

            bundles.Add(new ScriptBundle("~/Plugins").Include(
                //"~/scripts/gsdk-checkbox.js",
                "~/scripts/gsdk-morphing.js",
                "~/scripts/gsdk-radio.js",
                "~/scripts/gsdk-bootstrapswitch.js",
                "~/scripts/bootstrap-select.js",
                //"~/scripts/bootstrap-datepicker.js",
                "~/scripts/jquery.tagsinput.js",
                "~/scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/gsdCore").Include(
                "~/scripts/get-shit-done.js"));

            bundles.Add(new StyleBundle("~/cssFiles").Include(
                "~/Content/normalize.css",
                "~/Content/themes/base/jquery-ui.min.css",
                "~/Content/themes/base/jquery-ui.theme.min.css",
                "~/Content/bootstrap.min.css",
                "~/Content/gsdk.css",
                "~/Content/Site.css",
                "~/Content/DocumentIcons.css",
                "~/Content/DataTables/css/dataTables.bootstrap.css",
                "~/Content/DataTables/css/buttons.bootstrap.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}