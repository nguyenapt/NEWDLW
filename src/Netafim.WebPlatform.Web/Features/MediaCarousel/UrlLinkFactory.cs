using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.SuccessStory;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    public interface IUrlLinkFactory
    {
        bool IsSatisfy(IMediaCarousel carousel);

        string CreateLink(IMediaCarousel carousel);
    }

    public class SuccessPageUrlLinkFactory : IUrlLinkFactory
    {
        public bool IsSatisfy(IMediaCarousel carousel)
        {
            return carousel is SuccessStoryPage;
        }

        public string CreateLink(IMediaCarousel carousel)
        {
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            return $"href={urlResolver.GetUrl(((IContent) carousel).ContentLink)}";
        }
    }

    public class MediaCarouselUrlLinkFactory : IUrlLinkFactory
    {
        public bool IsSatisfy(IMediaCarousel carousel)
        {
            return carousel is MediaCarouselBaseBlock;
        }

        public string CreateLink(IMediaCarousel carousel)
        {
            var urlLink = ((MediaCarouselBaseBlock)carousel).Link;
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var url = urlLink != null ? urlResolver.GetUrl(new UrlBuilder(urlLink), EPiServer.Web.ContextMode.Default) : string.Empty;

            return !string.IsNullOrWhiteSpace(url) ? $"href={url} {urlLink.LinkTarget()}" : string.Empty;
        }
    }
}