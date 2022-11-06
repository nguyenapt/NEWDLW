using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.ProductFamily;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using System.ComponentModel.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Features.ProductFamilyQuickLink;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    [ContentType(DisplayName = "Product Category page", GUID = "81d4931e-9c1c-48b3-aeea-104a9dfb1e8a")]
    [AvailableContentTypes(
        Include =new[] {typeof(ProductFamilyPage), typeof(CriteriaContainerPage) },
        Exclude  = new[] { typeof(ProductCategoryPage) })]
    public class ProductCategoryPage : GenericContainerPage
    {
        [Display(Order = 20, Name = "Criteria Container Collection", GroupName =SharedTabs.Product)]
        [CultureSpecific]
        [AllowedTypes(typeof(CriteriaContainerPage))]
        public virtual ContentArea CriteriaCollection { get; set; }

        [Display(Order = 30, Name = "Icon for Product Family list (70x70)", GroupName = SharedTabs.Product)]
        [CultureSpecific]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(70, 70)]
        public virtual ContentReference ProductFamilyListIcon { get; set; }

        [AllowedTypes(AllowedTypes = new[] { typeof(IComponent), typeof(ProductFamilyQuickLinkBlock) })]
        [CultureSpecific]
        public override ContentArea Content { get; set; }
    }
}