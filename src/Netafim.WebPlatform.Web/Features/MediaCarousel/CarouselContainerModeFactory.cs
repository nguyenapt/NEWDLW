using System.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{

    public interface ICarouselContainerModeFactory
    {
        bool IsSatisfy(ICarouselMode carouselContainer);

        string CreateImageUrl(UrlHelper url, IMediaCarousel carousel);
    }

    public class CarouselBoxedModeFactory : ICarouselContainerModeFactory
    {
        public bool IsSatisfy(ICarouselMode carouselContainer)
        {
            return carouselContainer.IsBoxMode;
        }

        public string CreateImageUrl(UrlHelper url, IMediaCarousel carousel)
        {
            return url.CropImageUrl(carousel.Image, 1110, 620);
        }
    }

    public class CarouselFullWidthModeFactory : ICarouselContainerModeFactory
    {
        public bool IsSatisfy(ICarouselMode carouselContainer)
        {
            return !carouselContainer.IsBoxMode;
        }

        public string CreateImageUrl(UrlHelper url, IMediaCarousel carousel)
        {
            return url.CropImageUrl(carousel.Image, 1440, 620);
        }
    }
}