using System.Web;
using System.Web.Optimization;

namespace democode.mvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
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
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/colorbox").Include(
                "~/jQuery/colorbox-master/jquery.colorbox-min.js"));
            bundles.Add(new StyleBundle("~/Content/colorbox").Include(
                "~/jQuery/colorbox-master/example5/colorbox.css"));

            bundles.Add(new ScriptBundle("~/bundles/slides").Include(
                "~/jQuery/Slides-SlidesJS-3/examples/playing/js/jquery.slides.min.js"));
            bundles.Add(new StyleBundle("~/Content/slides").Include(
                "~/jQuery/Slides-SlidesJS-3/morrisonredslider.css"));

            bundles.Add(new ScriptBundle("~/bundles/timeline").Include(
                "~/jQuery/vertical-timeline/js/modernizr.js", 
                "~/jQuery/vertical-timeline/js/main.js"));
            bundles.Add(new StyleBundle("~/Content/timeline").Include(
                "~/jQuery/vertical-timeline/css/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/scrollto").Include(
                "~/jQuery/scrollTo/scrollTo.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/slider").Include(
                "~/jQuery/Slides-SlidesJS-3/examples/playing/js/jquery.slides.min.js"
                ));
            bundles.Add(new StyleBundle("~/Content/slider").Include(
                "~/jQuery/Slides-SlidesJS-3/morrisonredslider.css"
                ));

        }
    }
}
