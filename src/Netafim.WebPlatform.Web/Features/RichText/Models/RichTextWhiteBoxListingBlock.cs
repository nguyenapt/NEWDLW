using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    [ContentType(DisplayName = "Whitebox container block (Rich text component)", GroupName = GroupNames.Richtexts, GUID = "16159d5f-9b28-41ac-a46c-9ffd79338326", Description = "")]
    public class RichTextWhiteBoxListingBlock : RichTextComponent
    {
        [ScaffoldColumn(false)]
        public override string Title { get; set; }
        
        [AllowedTypes(typeof(IListableItem))]
        [CultureSpecific]
        public virtual ContentArea Items { get; set; }
    }
}