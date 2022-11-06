using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    [ContentType(DisplayName = "Product Family page", GUID = "35eacd26-4098-429b-8857-87454585684d")]
    public class ProductFamilyPage : GenericContainerPage
    {
        [Display(Order = 10, Name = "Property Collection", GroupName = SharedTabs.Product)]
        [CultureSpecific]
        [AllowedTypes(typeof(IProductFamilyProperty))]
        public virtual ContentArea PropertyCollection { get; set; }

        [Display(Name="Product Categories", Order =20, Description ="Specify Categories linked to this Product Family", GroupName = SharedTabs.Product)]
        [CultureSpecific]
        [AllowedTypes(typeof(ProductCategoryPage))]
        public virtual ContentArea ProductCategories { get; set; }

        [Display(Name = "Enable Detail Page", Order = 20, GroupName = SharedTabs.Product)]
        [CultureSpecific]
        public virtual bool EnableDetailPage { get; set; }
    }
}