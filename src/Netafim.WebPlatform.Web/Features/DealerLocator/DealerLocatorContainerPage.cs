using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    [ContentType(DisplayName = "Dealer location container", GroupName = GroupNames.Containers, GUID = "24977a82-5202-4a2b-879d-c543409f148d", Description = "Group the dealer locator template")]
    [AvailableContentTypes(Include = new[] { typeof(DealerLocatorPage) }, ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class DealerLocatorContainerPage : NoTemplatePageBase
    {

        [CultureSpecific]
        [Display(Name = "Pin icon on the map (28x34)", Order = 20)]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(28, 34)]
        public virtual ContentReference PinIcon { get; set; }

        [Display(Name = "Level of dealer group", Order = 30)]
        [SelectOne(SelectionFactoryType = typeof(DealerLevelFactory))]
        public virtual DealerLevel Level { get; set; }

        [Display(Name = "Color of dealer group", Order = 35)]
        [SelectOne(SelectionFactoryType = typeof(DealerColorFactory))]
        public virtual DealerColor Color { get; set; }

        [CultureSpecific]
        [Display(Name = "Category icon (40x40)", Order = 40)]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(40,40)]
        public virtual ContentReference LevelIcon { get; set; }

    }
}