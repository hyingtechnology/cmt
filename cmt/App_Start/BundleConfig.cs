using System.Web;
using System.Web.Optimization;

namespace cmt
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/validateunobtrusive").Include(
                        "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/countUp").Include(
                        "~/Scripts/countUp.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/jquery-ui.css",
                      "~/Content/bootstrap.css",
                      "~/Content/style.css",
                      "~/Content/main.css"));

            bundles.Add(new StyleBundle("~/Content/owlcss").Include(
                    "~/Content/owl.carousel.css",
                    "~/Content/owl.theme.css"));

            bundles.Add(new StyleBundle("~/Content/NoJSstylecss").Include(
                      "~/Content/NoJSstyle.css"));

            bundles.Add(new StyleBundle("~/Content/tinymce").Include(
                "~/Content/content.min.css"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                  "~/Content/jquery-ui.css",
                  "~/Content/bootstrap.css",
                  "~/Content/main-backend.css",
                  "~/Content/style-backend.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
