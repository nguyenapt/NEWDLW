using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    public class RichTextContainerViewModel
    {
        public RichTextContainerBlock Block { get; set; }

        public bool HasItems() => this.Block?.Items != null && this.Block.Items.Count > 0;
    }
}