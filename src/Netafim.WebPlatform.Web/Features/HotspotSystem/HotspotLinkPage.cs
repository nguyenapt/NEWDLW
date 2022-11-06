using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    [ContentType(DisplayName = "Hotspot link page", GUID = "c6353d8f-725a-42b4-b6a6-2e2d4e815bd0")]
    [AvailableContentTypes(Include = new[] { typeof(HotspotLinkNodePage) }, Exclude = new [] {typeof(HotspotPopupNodePage)}, ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class HotspotLinkPage : HotspotTemplateBase
    {
    }
}