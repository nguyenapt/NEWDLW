using EPiServer.Web;
using System.ComponentModel.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using EPiServer.Core;
using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;

namespace Netafim.WebPlatform.Web.Features.GeneralContactInfo
{

    [ContentType(DisplayName = "General Contact Information item", GroupName = GroupNames.Overview, GUID = "257c99ba-47f5-4e88-ad0c-8be32802e7d4", Description = "Display detail of a contact information")]
    public class GeneralContactInfoItemBlock : BaseBlock
    {
        [CultureSpecific]
        [Display(Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Order = 30)]
        [TinyMceSettings(typeof(TinyMceBaseConfiguration))]
        public virtual XhtmlString Information { get; set; }

        [CultureSpecific]
        [Display(Order = 40, Name = "Telephone contact")]
        public virtual string Phone { get; set; }

        [CultureSpecific]
        [Display(Order = 50, Name = "Fax contact")]
        public virtual string Fax { get; set; }

        [CultureSpecific]
        [Display(Name = "Email contact", Order = 60)]
        [EmailAddress]
        public virtual string MailTo { get; set; }

        [CultureSpecific]
        [Display(Name = "Text of email button", Order = 70)]
        public virtual string LinkTextButton { get; set; }

        #region Override base block

        [Ignore]
        public override string AnchorId { get; set; }

        [Ignore]
        public override string Watermark { get; set; }

        [Ignore]
        public override bool OnParallaxEffect { get; set; }

        #endregion
    }
}