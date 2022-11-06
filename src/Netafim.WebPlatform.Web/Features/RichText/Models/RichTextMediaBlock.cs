using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{

    [ContentType(DisplayName = "Media block (Rich text component)", GroupName = GroupNames.Richtexts, GUID = "a168aaa8-4787-4875-a4fb-417d48e41f7f", Description = "")]
    public class RichTextMediaBlock : RichTextComponent
    {
        [UIHint(UIHint.Image)]
        [CultureSpecific]
        [AllowedTypes(typeof(ImageData))]
        [Display(Name = "Image (722x527)", Order = 20, Description = "If video file is existed, it'll be displayed as the thumnail of video, otherwise it's displayed as normal image")]
        [ImageMetadata(722, 527)]
        public virtual ContentReference Image { get; set; }

        [UIHint(UIHint.Video)]
        [AllowedTypes(typeof(VideoData))]
        [CultureSpecific]
        [Display(Order = 20, Description = "")]
        public virtual ContentReference Video { get; set; }

        [CultureSpecific]
        [Display(Name = "Title (for SEO track)", Description = "The title using for SEO track", Order = 10)]
        public override string Title { get; set; }
        
        public bool IsVideoBlock()
        {
            return this.Video != null && this.Image != null;
        }
    }
}