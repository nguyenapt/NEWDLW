using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    public interface IDealerSettings
    {
        ContentReference DealerContainer { get; }

        int MinimalDealersWhenSearchByLocation { get; }

        int DealerRadius { get; }
    }
}
