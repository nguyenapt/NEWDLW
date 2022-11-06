using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.ProductFamily;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    public class CategoryListResultViewModel : PaginableBlockViewModel<ProductCategoryListingBlock, ProductCategoryPage>
    {
        public CategoryListResultViewModel(ProductCategoryListingBlock currentBlock, IPagedList<ProductCategoryPage> result) : base(currentBlock, result)
        {
        }

        public IEnumerable<ProductFamilyPage> ProductFamilies { get; set; }
    }
}