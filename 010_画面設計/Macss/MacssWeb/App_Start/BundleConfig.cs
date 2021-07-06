using System.Web.Optimization;

namespace MacssWeb
{
    public class BundleConfig
    {
        // バンドルの詳細については、https://go.microsoft.com/fwlink/?LinkId=301862 を参照してください
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 開発と学習には、Modernizr の開発バージョンを使用します。次に、実稼働の準備が
            // 運用の準備が完了したら、https://modernizr.com のビルド ツールを使用し、必要なテストのみを選択します。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/umd/popper.js",
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.bootstrap4.js",
                        "~/Scripts/DataTables/dataTables.fixedHeader.js",
                        "~/Scripts/DataTables/dataTables.fixedColumns.js",
                        "~/Scripts/DataTables/dataTables.buttons.js",
                        "~/Scripts/DataTables/buttons.flash.js",
                        "~/Scripts/jszip.js",
                        "~/Scripts/pdfmake/pdfmake.min.js",
                        "~/Scripts/pdfmake/vfs_fonts.js",
                        "~/Scripts/DataTables/buttons.colVis.js",
                        "~/Scripts/DataTables/buttons.bootstrap4.js",
                        "~/Scripts/DataTables/buttons.html5.js",
                        "~/Scripts/DataTables/buttons.print.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                      "~/Scripts/jquery.datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/macss").Include(
                      "~/Scripts/macss.datatable.js",
                      "~/Scripts/macss.datetimepicker.js",
                      "~/Scripts/macss.fileupload.js",
                      "~/Scripts/macss.collapse.js",
                      "~/Scripts/macss.startup.js",
                      "~/Scripts/macss.time.js"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css/fontawesome").Include(
                      "~/Content/fontawesome/css/all.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/datatables").Include(
                      //"~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/dataTables.bootstrap4.css",
                      "~/Content/DataTables/css/fixedHeader.bootstrap4.css",
                      "~/Content/DataTables/css/fixedColumns.bootstrap4.css",
                      "~/Content/DataTables/css/buttons.bootstrap4.css"));

            bundles.Add(new StyleBundle("~/Content/css/datetimepicker").Include(
                      "~/Content/jquery.datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/css/macss").Include(
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/scss").Include(
                      "~/Content/macss.css",
                      "~/Content/layout.css"));

        }
    }
}
