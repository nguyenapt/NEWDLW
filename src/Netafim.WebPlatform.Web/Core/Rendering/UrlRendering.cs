using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Extensions;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Core.Rendering
{
    public interface IUrlRendering
    {
        bool IsSatisfied(Url url);

        /// <summary>
        /// The method to get the target of href (html link)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        MvcHtmlString LinkTarget(Url url);
    }

    public class NullUrlRendering : IUrlRendering
    {
        public bool IsSatisfied(Url url)
        {
            return url == null || string.IsNullOrWhiteSpace(url.ToString()) || (url?.ToString()?.StartsWith("mailto:") == true);
        }

        public MvcHtmlString LinkTarget(Url url)
        {
            return MvcHtmlString.Empty;
        }
    }
    
    public class OpenNewTabRendering : IUrlRendering
    {
        protected readonly IContentLoader ContentLoader;

        public OpenNewTabRendering(IContentLoader contentLoader)
        {
            this.ContentLoader = contentLoader;
        }

        public bool IsSatisfied(Url url)
        {
            var content = url?.ToString().GetContent(ContentLoader);

            return content == null || content is MediaData;
        }

        public MvcHtmlString LinkTarget(Url url)
        {
            return MvcHtmlString.Create($"target=_blank");
        }
    }

    public class RedirectRendering : IUrlRendering
    {
        protected readonly IContentLoader ContentLoader;

        public RedirectRendering(IContentLoader contentLoader)
        {
            this.ContentLoader = contentLoader;
        }

        public bool IsSatisfied(Url url)
        {
            return url?.ToString().GetContent(ContentLoader) is PageData;
        }

        public MvcHtmlString LinkTarget(Url url)
        {
            return MvcHtmlString.Create($"target=_self");
        }
    }
}