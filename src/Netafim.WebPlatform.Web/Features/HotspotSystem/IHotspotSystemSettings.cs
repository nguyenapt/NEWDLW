using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    public interface IHotspotSystemSettings
    {
        ContentReference HotspotIconFallback { get; }
    }
}
