using System.Collections.Generic;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public interface IProductFamilyRepository
    {
        Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>> GetListPropertyCriteria(ProductCategoryPage curProductCategory);
        IEnumerable<string> GetCriteriaTypesDisplayInHeader(int[] criteriaTypeIds);
        IEnumerable<ProductFamilyPage> GetAllProductFamilyByCategoryPage(ProductCategoryPage curProductCategory);
    }
}