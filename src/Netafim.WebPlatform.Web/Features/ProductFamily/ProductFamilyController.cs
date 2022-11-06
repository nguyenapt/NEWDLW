using Castle.Core.Internal;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public class ProductFamilyController : ListingBaseBlockController<FamilyMatrixBlock, FamilyMatrixQueryViewModel, ProductFamilyPage>
    {
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly IProductFamilyRepository _productFamilyRepo;
        public ProductFamilyController(IContentLoader contentLoader,
            IPageService pageService,
            IFindSettings findSettings,
            IPageRouteHelper pageRouteHelper,
            IProductFamilyRepository productFamilyRepo)
            : base(contentLoader, pageService, findSettings)
        {
            _pageRouteHelper = pageRouteHelper;
            _productFamilyRepo = productFamilyRepo;
        }
        public override ActionResult Index(FamilyMatrixBlock currentContent)
        {
            var curProductCategory = _pageRouteHelper.Page as ProductCategoryPage;
            if (curProductCategory == null) return PartialView(FullViewPath(currentContent), new FamilyMatrixViewModel(currentContent));

            var model = new FamilyMatrixViewModel(currentContent)
            {
                AllCriteria = GetListCriteria(),
                ProductCategory = curProductCategory
            };

            return PartialView(FullViewPath(currentContent), model);
        }

        protected override ActionResult PopulateView(FamilyMatrixQueryViewModel query)
        {
            var composer = GetQueryComposer(query);

            var block = ContentLoader.Get<FamilyMatrixBlock>(new ContentReference(query.BlockId));
            var productFamilies = PageService.GetContents(FindSettings.MaxItemsPerRequest, composer.Compose(query)?.Expression);
            IPagedList<ProductFamilyPage> pagedList = new PagedList<ProductFamilyPage>(productFamilies.Cast<ProductFamilyPage>(), productFamilies.TotalMatching, productFamilies.TotalMatching, 1);

            var viewModel = new ProductFamilyListResultViewModel(block, pagedList)
            {
                CriteriaTypesInHeader = _productFamilyRepo.GetCriteriaTypesDisplayInHeader(query.CriteriaTypeIds),
                SelectedCriteriaIds = query.Criteria
            };
            ProductCategoryPage curProductCategory;
            if (ContentLoader.TryGet(new ContentReference(query.ProductCategoryId), out curProductCategory))
            {
                viewModel.ProductCategory = curProductCategory;
            }
            return PartialView(ResultViewPath(block), viewModel);
        }

        private Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>> GetListCriteria()
        {
            var curProductCategory = _pageRouteHelper.Page as ProductCategoryPage;
            if (curProductCategory == null) return new Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>>();
            return _productFamilyRepo.GetListPropertyCriteria(curProductCategory);
        }

        protected override string ResultViewPath(FamilyMatrixBlock model)
        {
            return string.Format(Core.Templates.Global.Constants.AbsoluteViewPathFormat, "ProductFamily", "_searchResult");
        }

    }
}