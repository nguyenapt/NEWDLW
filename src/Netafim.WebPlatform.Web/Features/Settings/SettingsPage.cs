using Netafim.WebPlatform.Web.Features.OfficeLocator;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.CountryLanguage;
using Netafim.WebPlatform.Web.Features.Navigation;
using Netafim.WebPlatform.Web.Infrastructure.Settings;
using System.Collections.Generic;
using EPiServer.DataAbstraction;
using Netafim.WebPlatform.Web.Features.FloatingCTA;
using Netafim.WebPlatform.Web.Features.DealerLocator;
using Netafim.WebPlatform.Web.Features.JobFilter;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using Netafim.WebPlatform.Web.Features.SystemConfigurator;

namespace Netafim.WebPlatform.Web.Features.Settings
{
    [ContentType(DisplayName = "Settings", Description = "System page to group various settings. Is not visible in the front-end.", GUID = "627488da-0e86-4bb4-a385-3c2625cda472", GroupName = GroupNames.Infrastructure)]
    [AvailableContentTypes(Availability.None, ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class SettingsPage : PageData, 
        ILayoutSettings, 
        IGoogleSettings, 
        IOfficeSettings, 
        IFloatingSharing, 
        IDealerSettings,
        ISystemConfiguratorSettings
    {
        [CultureSpecific]
        [Display(Name = "Cookie link", Order = 10, GroupName = SharedTabs.LayoutSettings, Description = "Specify the page that contains more information about cookies.")]
        public virtual ContentReference CookieLink { get; set; }

        [CultureSpecific]
        [Display(Name = "Navigation root", Order = 20, GroupName = SharedTabs.LayoutSettings, Description = "Specify the root of navigation.")]
        [AllowedTypes(typeof(NavigationContainerPage))]
        public virtual ContentReference NavigationRoot { get; set; }

        [CultureSpecific]
        [Display(Description = "Image to be used as the Logo .", Order = 30, GroupName = SharedTabs.LayoutSettings)]
        [AllowedTypes(typeof(ImageData))]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Logo { get; set; }

        [CultureSpecific]
        [Display(Name = "Hero Logo", Description = "Image to be used as the Logo when the page contains the Hero Banner.", Order = 40, GroupName = SharedTabs.LayoutSettings)]
        [AllowedTypes(typeof(ImageData))]
        [UIHint(UIHint.Image)]
        public virtual ContentReference HeroLogo { get; set; }

        [CultureSpecific]
        [Display(Name = "Career link", Order = 50, GroupName = SharedTabs.LayoutSettings, Description = "Specify the page that links to [Carreer] page in the header/footer.")]
        public virtual ContentReference CarreerLink { get; set; }

        [CultureSpecific]
        [Display(Name = "Other industries", Order = 60, GroupName = SharedTabs.LayoutSettings, Description = "Specify the pages which are listed as the [Other Industries] for this site.")]
        public virtual LinkItemCollection OtherIndustriess { get; set; }

        #region Social settings

        [CultureSpecific]
        [Display(Name = "Facebook link", Order = 70, GroupName = SettingsPageTabs.SocialSettings, Description = "Specify the Facebook link for this site.")]
        public virtual string FacebookLink { get; set; }

        [CultureSpecific]
        [Display(Name = "Linkedin link", Order = 80, GroupName = SettingsPageTabs.SocialSettings, Description = "Specify the LinkedIn link for this site.")]
        public virtual string LinkedInLink { get; set; }

        [CultureSpecific]
        [Display(Name = "Youtube link", Order = 90, GroupName = SettingsPageTabs.SocialSettings, Description = "Specify the Youtube link for this site.")]
        public virtual string YoutubeLink { get; set; }

        [CultureSpecific]
        [Display(Name = "Youtube channel", Order = 95, GroupName = SettingsPageTabs.SocialSettings, Description = "Specify the Youtube channel for this site.")]
        public virtual string YoutubeChannel { get; set; }

        [CultureSpecific]
        [Display(Name = "Instagram link", Order = 100, GroupName = SettingsPageTabs.SocialSettings, Description = "Specify the Instagram link for this site.")]
        public virtual string InstagramLink { get; set; }

        [CultureSpecific]
        [Display(Name = "Twitter link", Order = 110, GroupName = SettingsPageTabs.SocialSettings, Description = "Specify the Twitter link for this site.")]
        public virtual string TwitterLink { get; set; }

        [CultureSpecific]
        [Display(Name = "Facebook Script", Order = 110, GroupName = SettingsPageTabs.SocialSettings, Description = "Specify the script to integrate facebook")]
        [UIHint(UIHint.LongString)]
        public virtual string FacebookScript { get; set; }

        #endregion Social settings

        [Display(Name = "External Local Sites", Order = 120, GroupName = SystemTabNames.Content, Description = "Specify the external url for the local site corresponding to the language.")]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<LanguageLinkItem>))]
        public virtual IList<LanguageLinkItem> LocalSites { get; set; }

        #region Footer settings 

        [CultureSpecific]
        [Display(Name = "Contact Us link", Order = 50, GroupName = SharedTabs.LayoutSettings, Description = "Specify the page that links to [Contact Us] page in the header/footer.")]
        public virtual ContentReference ContatUs { get; set; }

        [CultureSpecific]
        [Display(Name = "Sub menu (Footer)", Order = 230, GroupName = SharedTabs.LayoutSettings, Description = "Sub menu of the footer")]
        public virtual LinkItemCollection SubFooter { get; set; }
        #endregion
        [CultureSpecific]
        [Display(Name = "Fallback Hotspot icon", Description = "Image to be used as the fallback Hotspot icon.", Order = 1120, GroupName = SettingsPageTabs.FallbackSettings)]
        [AllowedTypes(typeof(ImageData))]
        [UIHint(UIHint.Image)]
        public virtual ContentReference HotspotIcon { get; set; }

        [CultureSpecific]
        [Display(Name = "GTM ID", Order = 240, GroupName = SharedTabs.SeoInformation, Description = "Specify the ID that will be injected in the GTM script.")]
        public virtual string GtmId { get; set; }

        [CultureSpecific]
        [Display(Name = "Maximum number of links to show in on-this-level blocks.", Order = 10, GroupName = SharedTabs.Search)]
        [Range(1, 100)]
        public virtual int MaxLinksInOnThisLevelBlock { get; set; }

        #region Locator 

        [CultureSpecific]
        [Display(Name = "Google Maps api key", Order = 10, GroupName = SharedTabs.Search, Description = "Specify the Google Maps key.")]
        [UIHint(UIHint.LongString)]
        public virtual string GoogleMapsApiKey { get; set; }

        [CultureSpecific]
        [Display(Name = "Office container page", GroupName = SharedTabs.Search, Order = 20)]
        [AllowedTypes(typeof(OfficeLocatorContainerPage))]
        public virtual ContentReference OfficeContainer { get; set; }

        [CultureSpecific]
        [Display(Name = "Radius for searching the office (Km)", GroupName = SharedTabs.Search, Order = 30)]
        public virtual int Radius { get; set; }

        [CultureSpecific]
        [Display(Name = "Dealer container page", GroupName = SharedTabs.Search, Order = 50)]
        [AllowedTypes(typeof(NoTemplateContainerPage))]
        public virtual ContentReference DealerContainer { get; set; }

        [CultureSpecific]
        [Display(Name = "Radius for searching the dealer (Km)", GroupName = SharedTabs.Search, Order = 60)]
        public virtual int DealerRadius { get; set; }
        
        [CultureSpecific]
        [Display(Name = "The minimal dealers should be displayed when user search using Geolocation filter.", GroupName = SharedTabs.Search, Order = 70)]
        public virtual int MinimalDealersWhenSearchByLocation { get; set; }


        #endregion

        #region SeoInformation

        [CultureSpecific]
        [Display(Name = "GA Url", Order = 250, GroupName = SharedTabs.SeoInformation, Description = "Google analytics url")]
        public virtual string GaUrl { get; set; }

        [CultureSpecific]
        [Display(Name = "GA Tracking Id", Order = 260, GroupName = SharedTabs.SeoInformation, Description = "Google analytics key")]
        public virtual string GaTrackingId { get; set; }

        [CultureSpecific]
        [Display(Name = "Prefix GA Event Category", Order = 270, GroupName = SharedTabs.SeoInformation, Description = "Use prefix TEST for testing")]
        public virtual string PrefixGAEventCategory { get; set; }

        [Display(Name = "Title Suffix", Order = 275, GroupName = SharedTabs.SeoInformation)]
        [CultureSpecific]
        public virtual string TitleSuffix { get; set; }

        #endregion

        #region Email settings

        [CultureSpecific]
        [Display(Name = "Email Sender", Order = 280, GroupName = SharedTabs.EmailInformation, Description = "Email sender")]
        public virtual string EmailSenderAddress { get; set; }

        [CultureSpecific]
        [Display(Name = "SendGrid Api Key", Order = 290, GroupName = SharedTabs.EmailInformation, Description = "SendGrid Api Key")]
        public virtual string SendGridApiKey { get; set; }

        [CultureSpecific]
        [Display(Name = "SendGrid TemplateId", Order =300, GroupName = SharedTabs.EmailInformation, Description = "SendGrid TemplateId")]
        public virtual string SendGridTemplateId { get; set; }

        #endregion

        [Required]
        [AllowedTypes(typeof(EPiServer.Forms.Implementation.Elements.FormContainerBlock))]
        [Display(Name = "Lead form",
            Description = "Form to post the system configurator leads to.",
            GroupName = SharedTabs.SystemConfiguratorSettings,
            Order = 10)]
        public virtual ContentReference LeadForm { get; set; }

        [CultureSpecific]
        [Display(Name = "Shareaholic Site ID", Order = 120, GroupName = SettingsPageTabs.SocialSettings)]
        public virtual string ShareaholicSiteId { get; set; }

        [CultureSpecific]
        [Display(Name = "Job Departments", Order = 130, GroupName = SettingsPageTabs.JobSettings, Description = "Specify the Job Departments")]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<JobDepartment>))]
        public virtual IList<JobDepartment> JobDepartments { get; set; }

        [CultureSpecific]
        [Display(Name = "Job Locations", Order = 140, GroupName = SettingsPageTabs.JobSettings, Description = "Specify the Job Locations")]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<JobLocation>))]
        public virtual IList<JobLocation> JobLocations { get; set; }

        [CultureSpecific]
        [Display(Name = "Job Position", Order = 150, GroupName = SettingsPageTabs.JobSettings, Description = "Specify the Job Positions")]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<JobPosition>))]
        public virtual IList<JobPosition> JobPositions { get;  set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            MaxLinksInOnThisLevelBlock = 9;

            TitleSuffix = " - Netafim";

            // Google analytics default values
            GaUrl = "http://www.google-analytics.com/collect";
            GaTrackingId = "UA-5141975-1";
            PrefixGAEventCategory = "TEST";

            // SendGrid default values
            SendGridApiKey = "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";
            SendGridTemplateId = "eb22199a-570e-42de-a082-894ec090a79c";
            EmailSenderAddress = "noreply@delaware.pro";
        }
    }
}
