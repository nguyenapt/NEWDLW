using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Seo;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Globalization;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Shell;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    public abstract class PageBase : PageData, ISeoInformation
    {
        [Display(Order = 10, GroupName = SharedTabs.Preview)]
        [CultureSpecific]
        public virtual string Title { get; set; }

        #region Seo information
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Description", Order = 10, GroupName = SharedTabs.SeoInformation)]
        public virtual string SeoDescription { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Keywords (comma seperated)", Order = 20, GroupName = SharedTabs.SeoInformation)]
        public virtual string SeoKeywords { get; set; }

        [Display(Name = "Page title", Order = 30, GroupName = SharedTabs.SeoInformation)]
        [CultureSpecific]
        public virtual string SeoPageTitle { get; set; }

        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<AlternateLink>))]
        [Display(Name = "Custom alternate links", Order = 40, GroupName = SharedTabs.SeoInformation)]
        public virtual IList<AlternateLink> AlternateLinks { get; set; }

        #endregion

        [Display(Name = "Hide floating sharing", Description = "Show / hide floating cta on each page, default always show", GroupName = SharedTabs.LayoutSettings)]
        [CultureSpecific]
        public virtual bool HideFloatingSharing { get; set; }
    }
}