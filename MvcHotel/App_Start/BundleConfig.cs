using System.Web;
using System.Web.Optimization;

namespace MvcHotel
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                         "~/Scripts/modernizr-*",
                         "~/Scripts/jquery.slide.js",
                         "~/Scripts/menu-slider.js",
                         "~/Scripts/num-alignment.js",
                         "~/Scripts/popup.js",
                         "~/Scripts/bootstrap-datetimepicker.min.js",
                         "~/Scripts/bootstrap-datetimepicker.zh-CN.js",
                         "~/Scripts/jquery.validate.js",
                         "~/Scripts/jquery.validate.unobtrusive.js",
                         "~/Scripts/jquery.unobtrusive-ajax.js",
                         "~/Scripts/selectAll.js"
                         ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/Style.css",
                      "~/Content/fontawesome-all.css",
                      "~/Content/reset.css",
            "~/Content/bootstrap-datetimepicker.min.css"));
        }
    }
}
