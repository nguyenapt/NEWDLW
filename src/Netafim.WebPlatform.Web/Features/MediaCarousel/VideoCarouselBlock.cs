using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    [ContentType(DisplayName = "Video Carousel Block", GUID = "6cb99d87-1eff-4778-9c4b-897e457a27e8", Description = "Media carousel", GroupName = GroupNames.MediaCarousel)]
    public class VideoCarouselBlock : MediaCarouselBaseBlock
    {
        [Required]
        public override ContentReference Video { get; set; }
    }
}