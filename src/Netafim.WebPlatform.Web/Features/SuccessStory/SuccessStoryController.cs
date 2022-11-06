using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Features.MediaCarousel;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;

namespace Netafim.WebPlatform.Web.Features.SuccessStory
{
    [ContentOutputCache]
    public class SuccessStoryController : BasePageController<SuccessStoryPage>
    {
        public ActionResult Index(SuccessStoryPage currentPage)
        {
            var model = new SuccessStoryViewModel(currentPage)
            {
                CarouselContainerBlock = GetCarouselContainerBlock(currentPage)
            };

            return View(model);
        }

        private MediaCarouselContainerBlock GetCarouselContainerBlock(SuccessStoryPage currentPage)
        {
            var carouselBlock = new MediaCarouselContainerBlock {Items = new ContentArea()};
            carouselBlock.Items.Items.Add(new ContentAreaItem()
            {
                ContentLink = currentPage.ContentLink
            });

            return carouselBlock;
        }
    }
}