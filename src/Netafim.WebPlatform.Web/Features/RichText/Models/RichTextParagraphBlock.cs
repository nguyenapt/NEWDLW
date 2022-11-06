using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using Dlw.EpiBase.Content.Infrastructure.Extensions;
using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{

    [ContentType(DisplayName = "Paragraph block (Rich text component)", GroupName = GroupNames.Richtexts, GUID = "047d5109-fc63-45ac-9ccb-c6b74d377d00", Description = "Rich text block that allows editors can be composed the text content")]
    public class RichTextParagraphBlock : RichTextComponent
    {
        [Display(Order = 20)]
        [CultureSpecific]
        [TinyMceSettings(typeof(TinyMceBaseConfiguration))]
        public virtual XhtmlString Content { get; set; }
        
        public bool HasContent()
        {
            return !string.IsNullOrWhiteSpace(this.Content?.ToTextString());
        }
    }
}