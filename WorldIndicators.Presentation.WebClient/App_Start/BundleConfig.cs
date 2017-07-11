using System.Web;
using System.Web.Optimization;

namespace WorldIndicators.Presentation.WebClient
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/DataTables/jquery.dataTables.js",
                      "~/Scripts/moment-with-locales.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/locales/bootstrap-datepicker.es.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/initApp").Include(
                "~/Scripts/App/init.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/App/home.js"
            ));

            // ----------------------- CSS -----------------------

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap-datepicker.css",
                      "~/Content/font-awesome.css",
                      // "~/Content/DataTables/css/dataTables.bootstrap.css",
                      //"~/Content/DataTables/css/responsive.bootstrap.css",
                      "~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/jquery.dataTables_themeroller.css",
                      "~/Content/DataTables/css/responsive.dataTables.css",
                      "~/Content/site.css"
            ));
        }
    }
}
