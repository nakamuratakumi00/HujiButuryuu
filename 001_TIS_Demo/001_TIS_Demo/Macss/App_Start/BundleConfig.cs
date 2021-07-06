using System.Web;
using System.Web.Optimization;

namespace Macss
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/messages_ja.js"));

            // 開発と学習には、Modernizr の開発バージョンを使用します。次に、実稼働の準備が
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
            "~/Plugins/datetime-picker/js/bootstrap-datepicker.js",
            "~/Plugins/datetime-picker/locales/bootstrap-datepicker.ja.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/my-bootstrap-export").Include(
            "~/Plugins/my-bootstrap-table-export.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/table-export-xlsx").Include(
            "~/Plugins/tableExport.js",
            "~/Plugins/libs/js-xlsx/xlsx.core.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/table-export").Include(
            "~/Plugins/bootstrap-table/dist/extensions/export/tableExport.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/macss.css",
                      "~/Content/menu.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-datepicker").Include(
        "~/Plugins/datetime-picker/css/bootstrap-datepicker.css"));


            bundles.Add(new StyleBundle("~/Content/bootstraptable").Include(
              "~/Content/bootstrap-table.css",
              "~/Content/extensions/filter-control/bootstrap-table-filter-control.css"));


            bundles.Add(new ScriptBundle("~/bundles/bootstraptable").Include(
            "~/Content/bootstrap-table.js",
            "~/Content/bootstrap-table-ja-JP.js",
            "~/Content/extensions/export/bootstrap-table-export.js",
            "~/Content/extensions/multiple-sort/bootstrap-table-multiple-sort.js",
            "~/Content/extensions/filter-control/bootstrap-table-filter-control.js"
            ));


        }
    }
}
