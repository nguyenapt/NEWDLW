using Netafim.WebPlatform.Web.Features.OfficeLocator;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Microsoft.Ajax.Utilities;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Maintenance.Warmup;
using EPiServer.DataAbstraction;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    [ContentType(DisplayName = "Generic Content", 
        Description  = "Use this template when there is not a more specific template available for the content you want to create.", 
        GUID = "14364e49-8c4c-4791-9a25-bcfa636fa7a5")]
    [AvailableContentTypes(ExcludeOn = new[]
    {
        typeof(OfficeLocatorContainerPage),
        typeof(OfficeLocatorPage)
    })]
    public class GenericContainerPage : 
        PageBase, 
        IPreviewable, 
        IEnableCategorizable, 
        INavigationItem, 
        ICanBeSearched,
        IPreload
    {
        [AllowedTypes(typeof(IComponent))]
        [CultureSpecific]
        public virtual ContentArea Content { get; set; }

        #region IPreviewable

        [Display(Description = "Text that describes this page in short to be used in previews.", GroupName = SharedTabs.Preview)]
        [UIHint(UIHint.Textarea)]
        [CultureSpecific]
        public virtual XhtmlString Description { get; set; }

        [Display(Name = "Thumbnail", Description = "Image to be used in previews.", GroupName = SharedTabs.Preview)]
        [AllowedTypes(typeof(ImageData))]
        [UIHint(UIHint.Image)]
        [CultureSpecific]
        [ImageMetadata(620, 452)]
        public virtual ContentReference Thumnbail { get; set; }

        #endregion

        [Display(Name = "Banner Area", Description = "Container for items which will be displayed as the banner of the page.", GroupName = SharedTabs.LayoutSettings)]
        [CultureSpecific]
        [AllowedTypes(typeof(IComponent))]
        public virtual ContentArea BannerArea { get; set; }

        [Display(Name = "Navigation Image", Description = "Image to be used in Navigation.", GroupName = SharedTabs.LayoutSettings)]
        [AllowedTypes(typeof(ImageData))]
        [UIHint(UIHint.Image)]
        [CultureSpecific]
        public virtual ContentReference NavigationImage { get; set; }

        public string NavigationTitle => !string.IsNullOrEmpty(Title) ? Title : Name;

        public ContentReference NavigationImageLink => NavigationImage;

        public ContentReference NavigationLink => ContentLink;

        #region Search result

        [Ignore]
        string ICanBeSearched.Title => Title.IsNullOrWhiteSpace() ? Title : PageName;

        [Ignore]
        public string Summary => SeoDescription;

        [Ignore]
        public string Keywords => SeoKeywords;

        [Ignore]
        ContentReference ICanBeSearched.Image => Thumnbail;

        #endregion

        [CultureSpecific]
        [Display(Name = "Display In Sidebar Navigation", GroupName = "EPiServerCMS_SettingsPanel")]
        public virtual bool DisplayInSidebarNavigation { get; set; }

        [CultureSpecific]
        [Display(Name = "Display Sidebar Navigation", GroupName = "EPiServerCMS_SettingsPanel")]
        public virtual bool DisplaySidebarNavigation { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            DisplaySidebarNavigation = true;
            DisplayInSidebarNavigation = true;
        }       
    }
}