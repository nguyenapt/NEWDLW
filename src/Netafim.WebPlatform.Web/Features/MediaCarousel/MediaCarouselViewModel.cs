namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    public class MediaCarouselViewModel
    {
        public ICarouselMode CurrentBlock { get; set; }
        public IMediaCarousel CarouselItem { get; set; }

        public MediaCarouselViewModel(ICarouselMode currentBlock)
        {
            CurrentBlock = currentBlock;
        }
    }
}