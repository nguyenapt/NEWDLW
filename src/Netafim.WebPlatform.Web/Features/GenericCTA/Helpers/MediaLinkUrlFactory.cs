using System.IO;
using System.Linq;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Web.Routing;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{
    public class MediaLinkUrlFactory : LinkUrlLinkFactory
    {
        public MediaLinkUrlFactory(IContentLoader contentLoader) : base(contentLoader)
        {
        }

        public override string CreateLink(UrlHelper url, GenericCTABlock block)
        {
            return url.ContentUrl(block.Link);
        }

        public override bool IsSatisfied(Url url)
        {
            var content = this.GetContent(url);
            
            return content is MediaData;
        }

        public string GetMediaName(Url url)
        {
            if (url != null && url.Segments != null && url.Segments.Any())
            {
                var fileNameWithExtensions = url.Segments.Last();

                var dot = fileNameWithExtensions.LastIndexOf('.');

                return fileNameWithExtensions.Substring(0, dot);
            }

            return string.Empty;
        }
    }
}