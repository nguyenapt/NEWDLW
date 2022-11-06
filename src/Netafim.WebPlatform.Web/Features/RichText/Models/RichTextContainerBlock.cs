using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    [ContentType(DisplayName = "Rich text container", GroupName = GroupNames.Richtexts, GUID = "5fa4e0b6-b026-45ac-b80b-846c36e7942c", Description = "Component completely maintained with a WYSIWYG editor.")]
    public class RichTextContainerBlock : BaseBlock, IComponent
    {
        [Display(Name = "Mark the block is full screen.", Order = 20, Description = "Config the mode of rich text component is full or boxed mode, Default is boxed mode")]
        [CultureSpecific]
        public virtual bool IsFullWidth { get; set; }

        [Display(Name = "Mark the position of items are reversed on mobile mode.", Order = 30)]
        [CultureSpecific]
        public virtual bool ReverseItemsOnMobile { get; set; }

        [AllowedTypes(typeof(IRichTextColumnComponent))]
        [CultureSpecific]
        [Display(Description = "Container contains the rich text component", Order = 40)]
        public virtual ContentArea Items { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName();
    }
}