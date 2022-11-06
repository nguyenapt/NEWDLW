using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.ProductFamily;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    [ContentType(DisplayName = "Product Catalog page", GUID = "62db4c56-a9e4-4f4f-9be9-a27d8db433df")]
    [AvailableContentTypes(Include = new[] { typeof(ProductCategoryPage), typeof (ProductFamilyPage), typeof(CriteriaContainerPage) })]
    public class ProductCatalogPage : GenericContainerPage
    {
       
    }
}