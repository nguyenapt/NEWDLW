using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.ProductFamily;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    public class ProductCategoryListingController : ListingBaseBlockController<ProductCategoryListingBlock, ProductCategoryListingQueryViewModel, ProductCategoryPage>
    {
        public ProductCategoryListingController(IContentLoader contentLoader, IPageService pageService, IFindSettings findSettings)
            : base(contentLoader, pageService, findSettings)
        {
        }
        public override ActionResult Index(ProductCategoryListingBlock currentContent)
        {
            var model = new CategoryListBlockViewModel(currentContent)
            {
                ProductFamilies = GetProductFamilies()
            };
            return PartialView(FullViewPath(currentContent), model);
        }

        protected override ActionResult PopulateView(ProductCategoryListingQueryViewModel query)
        {
            var composer = GetQueryComposer(query);

            var block = ContentLoader.Get<ProductCategoryListingBlock>(new ContentReference(query.BlockId));

            var productCategories = PageService.GetContentsWithSorting(FindSettings.MaxItemsPerRequest, composer.Compose(query)?.Expression, composer.GetSortings(query));

            IPagedList<ProductCategoryPage> pagedList = new PagedList<ProductCategoryPage>(productCategories.Cast<ProductCategoryPage>(), productCategories.TotalMatching, productCategories.TotalMatching, 1);

            var viewModel = new CategoryListResultViewModel(block, pagedList);

            return PartialView(ResultViewPath(block), viewModel);
        }      
        private IEnumerable<ProductFamilyPage> GetProductFamilies()
        {
            var allFamilies = this.PageService.GetPages<ProductFamilyPage>(FindSettings.MaxItemsPerRequest);
            return allFamilies ?? Enumerable.Empty<ProductFamilyPage>();
        }

        protected override string ResultViewPath(ProductCategoryListingBlock model)
        {
            return string.Format(Core.Templates.Global.Constants.AbsoluteViewPathFormat, "ProductCategory", "_searchResult");
        }
    }
}