using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public class ProductFamilyListResultViewModel : PaginableBlockViewModel<FamilyMatrixBlock, ProductFamilyPage>
    {
        public ProductFamilyListResultViewModel(FamilyMatrixBlock currentBlock, IPagedList<ProductFamilyPage> result)
            : base(currentBlock, result)
        {
        }
        
        public IEnumerable<string> CriteriaTypesInHeader { get; set; }   
        
        public ProductCategoryPage ProductCategory { get; set; } 

        public IEnumerable<int> SelectedCriteriaIds { get; set; }
    }
}