using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Validation;
using EPiServer.DataAbstraction;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    [ContentType(DisplayName = "Dealer Locator block", GUID = "59b506db-cf4a-46c7-af46-036a948ff348", GroupName = GroupNames.Locator, Description = "")]
    public class DealerLocatorBlock : ListingBaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(Order = 30)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Order = 40, Name = "Show/hide map button's icon (20x20)")]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(20, 20)]
        public virtual ContentReference ShowHideMapIcon { get; set; }
        
        [CultureSpecific]
        [Display(Name = "Contact form", Description = "Contact form as an overlay form", GroupName = SystemTabNames.Content, Order = 50)]
        [AllowedTypes(typeof(GeneralFormContainerBlock))]
        [ContentAreaMaxItems(1)]
        public virtual ContentArea ContactForm { get; set; }

        public string ComponentName => this.GetComponentName(this.Title);
    }
}