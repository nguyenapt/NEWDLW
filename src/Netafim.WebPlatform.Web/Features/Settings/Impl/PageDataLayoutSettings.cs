using System.Collections.Generic;
using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Netafim.WebPlatform.Web.Core.GoogleAnalytics;
using Netafim.WebPlatform.Web.Core.Services;
using Netafim.WebPlatform.Web.Features.CountryLanguage;
using Netafim.WebPlatform.Web.Features.FloatingCTA;
using Netafim.WebPlatform.Web.Features.HotspotSystem;
using Netafim.WebPlatform.Web.Features.Layout;
using Netafim.WebPlatform.Web.Features.Navigation;
using Netafim.WebPlatform.Web.Features.OnThisLevel;
using Netafim.WebPlatform.Web.Features.SystemConfigurator;
using Netafim.WebPlatform.Web.Infrastructure.Settings;

namespace Netafim.WebPlatform.Web.Features.Settings.Impl
{
    public class PageDataLayoutSettings : 
        PageDataSettings, 
        ILayoutSettings, 
        IHeaderSettings, 
        ISocialMediaSettings, 
        INavigationSettings, 
        ICountryLanguageSelectionSettings,
        IHotspotSystemSettings,
        IFooterSettings,
        IGoogleAnalyticsSettings,
        IOnThisLevelSettings,
        IEmailSettings,
        IFloatingSharing,
        ISystemConfiguratorSettings
    {
        public ContentReference CookieLink => SettingsPage.CookieLink;

        public ContentReference LogoLink => SettingsPage.Logo;

        public ContentReference CarreerLink => SettingsPage.CarreerLink;

        public ContentReference ContactUsLink => SettingsPage.ContatUs;

        public LinkItemCollection OtherIndustries => SettingsPage.OtherIndustriess;

        public string FacebookLink => SettingsPage.FacebookLink;

        public string YoutubeLink => SettingsPage.YoutubeLink;

        public string TwitterLink => SettingsPage.TwitterLink;

        public string LinkedInLink => SettingsPage.LinkedInLink;

        public string InstagramLink => SettingsPage.InstagramLink;

        public string YoutubeChannel => SettingsPage.YoutubeChannel;

        public ContentReference NavigationRoot => SettingsPage.NavigationRoot;

        public ContentReference HeroLogolink => SettingsPage.HeroLogo;

        public IList<LanguageLinkItem> LocalSites => SettingsPage.LocalSites;

        public ContentReference HotspotIconFallback => SettingsPage.HotspotIcon;

        public LinkItemCollection SubFooter => SettingsPage.SubFooter;

        public ContentReference ContatUs => SettingsPage.ContatUs;

        public string GtmId => SettingsPage.GtmId;

        public string GaUrl => SettingsPage.GaUrl;

        public string GATrackingId => SettingsPage.GaTrackingId;

        public string PrefixGAEventCategory => SettingsPage.PrefixGAEventCategory;

        public string EmailSenderAddress => SettingsPage.EmailSenderAddress;

        public string SendGridApiKey => SettingsPage.SendGridApiKey;

        public string SendGridTemplateId => SettingsPage.SendGridTemplateId;

        public PageDataLayoutSettings(IContentRepository contentRepository) : base(contentRepository) {  }

        public int MaxLinksInOnThisLevelBlock => SettingsPage.MaxLinksInOnThisLevelBlock;

        public string ShareaholicSiteId => SettingsPage.ShareaholicSiteId;

        public ContentReference LeadForm => SettingsPage.LeadForm;
    }
}