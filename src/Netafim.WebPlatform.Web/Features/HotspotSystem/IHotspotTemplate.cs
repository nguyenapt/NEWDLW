using EPiServer.Core;
using EPiServer.Shell;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    public interface IHotspotTemplate : IContent
    {
        string Title { get; }

        string SubTitle { get; }

        ContentReference Image { get; }

        int ImageWidth { get; }

        int ImageHeight { get; }

        ContentReference HotspotIcon { get; }
    }

    // Enable dropping content based on interface in content area
    [UIDescriptorRegistration]
    public class IHotspotTemplateUIDescriptor : UIDescriptor<IHotspotTemplate>
    { }
}
