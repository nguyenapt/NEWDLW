using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    public abstract class RichTextComponent : ItemBaseBlock, IRichTextColumnComponent
    {
        [CultureSpecific]
        [Display(Name = "Title", Order = 10)]
        public virtual string Title { get; set; }
    }
}