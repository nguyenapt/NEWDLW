using EPiServer.Web.Routing;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using System.Web.Mvc;
using EPiServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Web;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{

    public class InternalLinkUrlFactory : LinkUrlLinkFactory
    {
        public InternalLinkUrlFactory(IContentLoader contentLoader)
            : base(contentLoader)
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

            return content is PageData;
        }
    }
}