using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public class FamilyMatrixViewModel : IBlockViewModel<FamilyMatrixBlock>
    {
        public FamilyMatrixViewModel(FamilyMatrixBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
        public FamilyMatrixBlock CurrentBlock { get; set; }

        public ProductCategoryPage ProductCategory { get; set; }

        public Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>> AllCriteria { get; set; }
    }
}