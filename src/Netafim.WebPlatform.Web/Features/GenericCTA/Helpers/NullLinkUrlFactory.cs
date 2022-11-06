using System;
using System.Web.Mvc;
using EPiServer;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{
    /// <summary>
    /// Default handle for null value
    /// </summary>
    public class NullLinkUrlFactory : LinkUrlLinkFactory
    {
        public NullLinkUrlFactory(IContentLoader contentLoader) : base(contentLoader)
        {
        }

        public override string CreateLink(UrlHelper url, GenericCTABlock block)
        {
            return string.Empty;
        }

        public override bool IsSatisfied(Url url)
        {
            return url == null;
        }
    }
}