using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Validation;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    [ContentType(DisplayName = "Hero Banner Container Block", GUID = "dc163c96-3de7-468b-bbd6-fdfaee587416", Description = "Hero banner carousel", GroupName = GroupNames.HeroBanerCarousel)]
    public class HeroBannerContainerBlock : MediaCarouselContainerBlock
    {
        [CultureSpecific]
        [Display(Description = "Hero banner items", GroupName = SystemTabNames.Content, Order = 10)]
        [AllowedTypes(AllowedTypes = new[] { typeof(HeroBannerBlock) })]
        [ContentAreaMaxItems(3)]
        public override ContentArea Items { get; set; }

        [Ignore]
        public override bool IsBoxMode { get; set; }
    }
}