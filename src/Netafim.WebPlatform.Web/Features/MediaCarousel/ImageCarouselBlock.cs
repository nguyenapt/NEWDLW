using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    [ContentType(DisplayName = "Image Carousel Block", GUID = "8b2c2440-fe36-4c58-93f9-62afdab4acbb", Description = "Media carousel", GroupName = GroupNames.MediaCarousel)]
    public class ImageCarouselBlock : MediaCarouselBaseBlock
    {
        [Ignore]
        public override ContentReference Video { get; set; }

        [Ignore]
        public override bool OnAutoPlay { get; set; }

        [Ignore]
        public override bool ShowContentWhilePlaying { get; set; }
    }
}