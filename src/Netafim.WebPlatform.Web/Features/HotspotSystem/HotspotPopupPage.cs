using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    [ContentType(DisplayName = "Hotspot popup page", GUID = "d0d9b116-e680-4e69-947e-7af34d0f6aaa")]
    [AvailableContentTypes(Include = new[] { typeof(HotspotPopupNodePage) }, ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class HotspotPopupPage : HotspotTemplateBase
    {
    }
}