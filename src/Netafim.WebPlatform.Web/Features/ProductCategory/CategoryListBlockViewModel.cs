using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using Netafim.WebPlatform.Web.Features.ProductFamily;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    public class CategoryListBlockViewModel  : IBlockViewModel<ProductCategoryListingBlock>
    {
        public CategoryListBlockViewModel(ProductCategoryListingBlock block)
        {
            this.CurrentBlock = block;
        }

        public IEnumerable<ProductFamilyPage> ProductFamilies { get; set; }

        public ProductCategoryListingBlock CurrentBlock { get; }
    }
}