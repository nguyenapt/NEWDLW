using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using Netafim.WebPlatform.Web.Core.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    [ContentType(DisplayName = "White box block (Whitebox component)", GroupName = GroupNames.Richtexts, GUID = "9a7ea5e6-bbfa-40d9-bf36-999416eddf5d", Description = "")]
    public class RichTextWhiteBoxBlock : BaseBlock, IListableItem, ICTAComponent
    {
        [CultureSpecific]
        [Display(Order = 10)]
        public virtual string Title { get; set; }

        [AllowedTypes(typeof(ImageData))]
        [Display(Order = 20, Name = "Icon of the block (54x54)")]
        [CultureSpecific]
        [ImageMetadata(54, 54)]
        public virtual ContentReference Image { get; set; }

        [UIHint(UIHint.Textarea)]
        [Display(Order = 30)]
        [CultureSpecific]
        public virtual string Description { get; set; }

        [Display(Order = 40)]
        [CultureSpecific]
        public virtual ContentReference Link { get; set; }

        [Display(Order = 50, Name = "Displayed text for the link")]
        [CultureSpecific]
        public virtual string LinkText { get; set; }

        [ScaffoldColumn(false)]
        public override string Watermark { get; set; }

        [Ignore]
        public override bool OnParallaxEffect { get; set; }
    }
}