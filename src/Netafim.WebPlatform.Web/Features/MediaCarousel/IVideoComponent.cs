using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    public interface IVideoComponent : IContentData
    {
        [ImageMetadata(1440, 620)]
        ContentReference Image { get; set; }
        ContentReference Video { get; set; }
        bool OnAutoPlay { get; set; }
        bool ShowContentWhilePlaying { get; set; }
    }
}
