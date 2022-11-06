using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    [ContentType(DisplayName = "Hero Banner Block", GUID = "7af10786-0d05-45a3-bbe8-6c6ca4c79aa9", Description = "Hero banner carousel", GroupName = GroupNames.HeroBanerCarousel)]
    public class HeroBannerBlock : MediaCarouselBaseBlock
    {
        [CultureSpecific]
        [Display(Name = "Text up", Description = "Description display as line up", GroupName = SystemTabNames.Content, Order = 20)]
        [TinyMceSettings(typeof(TinyMceBaseConfiguration))]
        public virtual XhtmlString TextUp { get; set; }

        [CultureSpecific]
        [Display(Name = "Disable blue slash", Description = "The blue slash in the top banner banner divide between Text up and Text down", GroupName = SystemTabNames.Content, Order = 25)]
        public virtual bool DisableBlueSlash { get; set; }

        [CultureSpecific]
        [Display(Name = "Text down", Description = "Description display as line down", GroupName = SystemTabNames.Content, Order = 30)]
        [TinyMceSettings(typeof(TinyMceBaseConfiguration))]
        public virtual XhtmlString TextDown { get; set; }

        [Display(Name = "Image (1440x930)", Description = "Image display as image carousel and full width (1440x930)", GroupName = SystemTabNames.Content, Order = 50)]
        [ImageMetadata(1440, 930)]
        [Required]
        public override ContentReference Image { get; set; }

        [Ignore]
        public override XhtmlString Text { get; set; }

        [Display(Name = "Button text", GroupName = SystemTabNames.Content, Order = 35)]
        public override string LinkText { get; set; }
    }
}