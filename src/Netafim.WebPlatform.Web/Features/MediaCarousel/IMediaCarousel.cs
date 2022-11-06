using EPiServer.Core;
using EPiServer.Shell;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    public interface IMediaCarousel : IVideoComponent
    {
        string Title { get; set; }
        XhtmlString Text { get; set; }
        XhtmlString Quote { get; set; }
        string LinkText { get; set; }
    }

    // Enable dropping content based on interface in content area
    [UIDescriptorRegistration]
    public class IMediaCarouselUIDescriptor : UIDescriptor<IMediaCarousel>
    { }
}
