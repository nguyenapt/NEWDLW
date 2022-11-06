using System;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    [ContentType(DisplayName = "No Template Page", GUID = "1503E747-7C24-4C0C-B523-F3234B182341")]
    public class  NoTemplatePage : NoTemplatePageBase, INavigationItem, ICanBeSearched
    {
        public string NavigationTitle => Name;

        public ContentReference NavigationImageLink => ContentReference.EmptyReference;

        public ContentReference NavigationLink => ContentLink;

        [CultureSpecific]
        [Display(Name = "Display In Sidebar Navigation", GroupName = "EPiServerCMS_SettingsPanel")]
        public virtual bool DisplayInSidebarNavigation { get; set; }

        public bool DisplaySidebarNavigation => false;

        public string Title => Name;

        public string Summary => Name;
        public string Keywords => string.Empty;

        public ContentReference Image => ContentReference.EmptyReference;

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            this.DisplayInSidebarNavigation = false;
        }
    }
}