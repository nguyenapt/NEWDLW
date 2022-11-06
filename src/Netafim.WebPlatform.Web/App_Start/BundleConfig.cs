using System.Web.Optimization;

namespace Netafim.WebPlatform.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle("~/netafim-css")
                .Include("~/Content/styles/main.css", new CssRewriteUrlTransform());

            bundles.Add(styleBundle);

            bundles.Add(new ScriptBundle("~/jquery")
                .Include("~/Content/scripts/vendor/jquery/jquery.js")
            );

            // NEXT refactor to include folder but exclude jquery
            bundles.Add(new ScriptBundle("~/vendor-scripts")
                .Include("~/Content/scripts/vendor/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.js")
                .Include("~/Content/scripts/vendor/jquery-validation/jquery.validate.js")
                .Include("~/Content/scripts/vendor/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js")
            );

            bundles.Add(new ScriptBundle("~/custom-scripts")
                .IncludeDirectory("~/Content/scripts/", "*.js", false)
                .IncludeDirectory("~/Features", "*.js", true)
            );
        }

    }
}