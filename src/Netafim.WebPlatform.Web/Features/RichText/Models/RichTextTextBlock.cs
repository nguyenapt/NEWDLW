using Dlw.EpiBase.Content.Infrastructure.Extensions;
using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{

    [ContentType(DisplayName = "Text block (Rich text component)", GroupName = GroupNames.Richtexts, GUID = "68278300-488c-4f6d-ab1e-3e83a7ede116", Description = "Component completely maintained with a WYSIWYG editor.  A styled CTA button can be optionally configured")]
    public class RichTextTextBlock : RichTextComponent, ICTAComponent
    {
        [Display(Order = 20)]
        [CultureSpecific]
        [TinyMceSettings(typeof(TinyMceBaseConfiguration))]
        public virtual XhtmlString Content { get; set; }
        
        [Display(Order = 30, Description = "Config the reference link to specific content")]
        [CultureSpecific]
        public virtual ContentReference Link { get; set; }

        [Display(Order = 40, Name = "Display for the link")]
        [CultureSpecific]
        public virtual string LinkText { get; set; }
                
        public bool HasContent()
        {
            return !string.IsNullOrWhiteSpace(this.Content?.ToTextString());
        }
    }
}