using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using EPiServer.Core;
using System.ComponentModel.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using Dlw.EpiBase.Content.Infrastructure.Extensions;
using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using Netafim.WebPlatform.Web.Core.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    [ContentType(DisplayName = "Rich text with image and text block(Rich text component)", GroupName = GroupNames.Richtexts, GUID = "87bb7c23-d776-4ed1-a52a-3fcf98356d0f", Description = "The block with text only display on left with 75% width")]
    public class RichTextWithImageAndTextBlock : RichTextComponent, ICTAComponent
    {
        [Display(Order = 20)]
        [CultureSpecific]
        [TinyMceSettings(typeof(TinyMceBaseConfiguration))]
        public virtual XhtmlString Content { get; set; }

        [AllowedTypes(typeof(ImageData))]
        [Display(Name = "Image (620x452)", Order = 30, Description = "Image will be displayed on the right with small display")]
        [CultureSpecific]
        [ImageMetadata(620, 452)]
        public virtual ContentReference Image { get; set; }

        [Display(Name = "Link to reference content", Order = 40)]
        [CultureSpecific]
        public virtual ContentReference Link { get; set; }

        [Display(Name = "Displayed text for the link", Order = 50)]
        [CultureSpecific]
        public virtual string LinkText { get; set; }

        public bool HasContent()
        {
            return !string.IsNullOrWhiteSpace(this.Content?.ToTextString());
        }
    }
}