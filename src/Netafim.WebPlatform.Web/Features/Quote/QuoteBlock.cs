using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using System.ComponentModel.DataAnnotations;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using Dlw.EpiBase.Content.Infrastructure.Extensions;
using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.Quote
{
    [ContentType(DisplayName = "Quote block", GUID = "86640ff7-38c5-47b4-bdbf-020ec3c61001", Description = "This component has as goal to highlight a certain quote", GroupName = GroupNames.Overview)]
    public class QuoteBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(Name = "The quote content is in smaller size", Order = 20)]
        public virtual bool IsSmallQuoteText { get; set; }

        [CultureSpecific]
        [Display(Name = "The image of quote (110x110)", Order = 30)]
        [ImageMetadata(110, 110)]
        [AllowedTypes(typeof(ImageData))]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(Description = "The quote content", Order = 40)]
        [TinyMceSettings(typeof(BoldTinyMceConfiguration))]
        public virtual XhtmlString Content { get; set; }

        [CultureSpecific]
        [Display(Description = "The quote author", Order = 50)]
        public virtual string Author { get; set; }

        [Ignore]
        public override string Watermark { get; set; }

        [Ignore]
        public override bool OnParallaxEffect { get; set; }

        public bool HasContent() => !string.IsNullOrWhiteSpace(this.Content?.ToTextString());
        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName();
    }
}