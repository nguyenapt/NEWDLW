using System.Web.Mvc;
using EPiServer;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{
    public class OverlayLinkUrlFactory : LinkUrlLinkFactory
    {
        public OverlayLinkUrlFactory(IContentLoader contentLoader) : base(contentLoader)
        {
        }

        public override string CreateLink(UrlHelper url, GenericCTABlock block)
        {
            return "javascript:void(0)";
        }

        public override bool IsSatisfied(Url url)
        {
            return url != null && url == Constants.DefaultOverlayUrl;
        }

        public MvcHtmlString GetAdditionalCtaButtonAttributes(GenericCTABlock block)
        {
            var overlayAttribute = $"data-open='popup-overlay'";

            return MvcHtmlString.Create(overlayAttribute);
        }
    }
}