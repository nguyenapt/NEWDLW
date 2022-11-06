using EPiServer.SpecializedProperties;
using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public interface IFooterSettings
    {
        ContentReference ContatUs { get; }
        LinkItemCollection SubFooter { get; }
    }
}
