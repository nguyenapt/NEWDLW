using EPiServer.Core;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{

    public abstract class LinkUrlLinkFactory : IUrlLinkFactory
    {
        private readonly IContentLoader _contentLoader;

        protected LinkUrlLinkFactory(IContentLoader contentLoader)
        {
            this._contentLoader = contentLoader;
        }

        public abstract string CreateLink(UrlHelper url, GenericCTABlock block);
        
        public abstract bool IsSatisfied(Url url);

        protected IContent GetContent(Url url)
        {
            return url?.ToString().GetContent(this._contentLoader);
        }
    }
}