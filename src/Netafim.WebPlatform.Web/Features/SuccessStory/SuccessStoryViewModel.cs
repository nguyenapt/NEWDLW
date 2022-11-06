using Netafim.WebPlatform.Web.Features.MediaCarousel;

namespace Netafim.WebPlatform.Web.Features.SuccessStory
{
    public class SuccessStoryViewModel
    {
        public SuccessStoryViewModel(SuccessStoryPage currentPage)
        {
            CurrentPage = currentPage;
        }
        public SuccessStoryPage CurrentPage { get; }
        public MediaCarouselContainerBlock CarouselContainerBlock { get; set; }
    }
}