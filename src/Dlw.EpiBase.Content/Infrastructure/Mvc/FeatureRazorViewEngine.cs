using System.Linq;
using System.Web.Mvc;

namespace Dlw.EpiBase.Content.Infrastructure.Mvc
{
    public class FeatureRazorViewEngine : RazorViewEngine
    {
        public const string FeatureLocation = "~/Features";

        public FeatureRazorViewEngine()
        {
            MasterLocationFormats = new[]
            {
                FeatureLocation + "/Shared/Views/{0}.cshtml",
                FeatureLocation + "/Shared/Views/{0}.vbhtml"
            };

            var viewLocations = new[]
            {
                FeatureLocation + "/{1}/Views/{0}.cshtml",
                FeatureLocation + "/{1}/Views/{0}.vbhtml",
                FeatureLocation + "/_Shared/Views/{0}.cshtml",
                FeatureLocation + "/_Shared/Views/{0}.vbhtml"
            };

            ViewLocationFormats = ViewLocationFormats.Union(viewLocations).ToArray();

            PartialViewLocationFormats = PartialViewLocationFormats.Union(viewLocations).ToArray();
        }
    }
}