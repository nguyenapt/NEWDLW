using EPiServer;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{
    public static class Constants
    {
        private readonly static Url _defaultUrl = new Url("OverlayUrl");
        public static Url DefaultOverlayUrl => _defaultUrl;
    }
}