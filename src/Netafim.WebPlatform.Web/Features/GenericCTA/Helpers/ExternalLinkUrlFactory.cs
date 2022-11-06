using EPiServer.Web.Routing;
using EPiServer.ServiceLocation;
using EPiServer.DataAbstraction;
using EPiServer.Web.Mvc.Html;
using System.Web.Mvc;
using EPiServer;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{

    public class ExternalLinkUrlFactory : LinkUrlLinkFactory
    {
        public ExternalLinkUrlFactory(IContentLoader contentLoader) : base(contentLoader)
        {
        }

        public override string CreateLink(UrlHelper url, GenericCTABlock block)
        {
            var anchor = !string.IsNullOrWhiteSpace(block.LinkAnchor) ? $"#{block.LinkAnchor}" : "";
            return $"{url.ContentUrl(block.Link)}{anchor}";
        }

        public override bool IsSatisfied(Url url)
        {            
            var content = this.GetContent(url);

            return content == null && url != null && url != Constants.DefaultOverlayUrl;
        }
    }
}