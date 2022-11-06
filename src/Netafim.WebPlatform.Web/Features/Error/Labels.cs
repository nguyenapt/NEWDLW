using DbLocalizationProvider;

namespace Netafim.WebPlatform.Web.Features.Error
{
    [LocalizedResource]
    public class Labels
    {
        public static string Watermark => "Error 404";

        public static string Title => "The page you were looking for <strong>can not be found</strong>";

        public static string Message => "Please try to find your content through the navigation or search.";
    }
}