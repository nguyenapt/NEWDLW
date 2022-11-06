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
    public class EmailUrlLinkFactory : IUrlLinkFactory
    {
        public string CreateLink(UrlHelper url, GenericCTABlock block)
        {
            return url.ContentUrl(block.Link);
        }
        
        public bool IsSatisfied(Url url)
        {
            return url?.ToString()?.StartsWith("mailto:") == true;
        }
    }
}