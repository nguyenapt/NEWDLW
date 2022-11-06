using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Validation;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;
using Netafim.WebPlatform.Web.Features.GenericCTA.Helpers;

namespace Netafim.WebPlatform.Web.Features.GenericCTA
{
    [ContentType(DisplayName = "Overlay CTA component", GUID = "37ef722a-9de1-4f70-a272-6b2b000a744f", Description = "A 'overlay' CTA is an important element on a lot of pages.  When the user clicks this item, he is not redirected to the page but form is opened in an overlay on top of the current page")]
    public class OverlayCTABlock : GenericCTABlock
    {
        [Ignore]
        public override bool IsInlineMode => false;

        [Ignore]
        public override Url Link => Constants.DefaultOverlayUrl;

        [Ignore]
        public override string LinkAnchor { get; set; }

        [CultureSpecific]
        [Display(Name = "Container for overlay content", Order = 80)]
        [AllowedTypes(typeof(GeneralFormContainerBlock))]
        [ContentAreaMaxItems(1)]
        public virtual ContentArea OverlayContent { get; set; }
    }
}