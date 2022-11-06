using Netafim.WebPlatform.Web.Features.OfficeLocator;
using EPiServer;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Features.DealerLocator;

namespace Netafim.WebPlatform.Web.Features.Settings.Impl
{
    public class PageDataSearchSettings : PageDataSettings, IGoogleSettings, IOfficeSettings, IDealerSettings
    {
        public PageDataSearchSettings(IContentRepository contentRepository) : base(contentRepository)
        {
        }

        public string GoogleMapsApiKey => SettingsPage.GoogleMapsApiKey;

        public int Radius => SettingsPage.Radius;

        public ContentReference OfficeContainer => SettingsPage.OfficeContainer;

        public ContentReference DealerContainer => SettingsPage.DealerContainer;

        public int DealerRadius => SettingsPage.DealerRadius;

        public int MinimalDealersWhenSearchByLocation => SettingsPage.MinimalDealersWhenSearchByLocation;
    }
}