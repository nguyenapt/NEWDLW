using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.GenericCTA
{
    [ContentType(DisplayName = "Generic CTA block", GUID = "42ce4444-6ce2-4b8e-be12-bb27655fd8d7", Description = "The webmaster must be able to configure a CTA that links to the page and position that is relevant.")]
    public class GenericCTABlock : BaseBlock, IComponent
    {
        [Display(Order = 20, GroupName = SystemTabNames.Content, Name = "Mark the CTA block is inline mode or not. Default (false) is not inline mode.")]
        [CultureSpecific]
        public virtual bool IsInlineMode { get; set; }

        [AllowedTypes(typeof(ImageData))]
        [Display(Name = "Image (70x70)", Order = 20)]
        [CultureSpecific]
        [ImageMetadata(70, 70)]
        public virtual ContentReference Icon { get; set; }

        [CultureSpecific]
        [Display(Order = 30)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Order = 40)]
        [UIHint(UIHint.Textarea)]
        public virtual string Description { get; set; }

        [CultureSpecific]
        [Display(Description = "This is the URL that the anchor will go to.", Order = 50)]
        public virtual Url Link { get; set; }

        [Display(Name = "Displayed text for the link", Order = 60)]
        [CultureSpecific]
        public virtual string LinkText { get; set; }

        [CultureSpecific]
        [Display(Name = "Anchor Id of Link (optional)", Description = "Set the anchor Id of content that's navigated by the Link", Order = 70)]
        public virtual string LinkAnchor { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);

        [Ignore]
        public override string VerticalText { get; set; }
    }
}