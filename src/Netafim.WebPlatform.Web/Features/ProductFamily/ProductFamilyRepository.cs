using Castle.Core.Internal;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public class ProductFamilyRepository : IProductFamilyRepository
    {
        protected readonly IContentLoader ContentLoader;
        protected readonly IPageService PageService;
        protected readonly IFindSettings FindSettings;
        protected readonly IClient ClientSearch;
        private const int MaxCriteriaNumber = 4;

        public ProductFamilyRepository(IContentLoader contentLoader, IPageService pageService, IFindSettings findSettings, IClient clientSearch)
        {
            ContentLoader = contentLoader;
            PageService = pageService;
            FindSettings = findSettings;
            ClientSearch = clientSearch;
        }

        public Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>> GetListPropertyCriteria(ProductCategoryPage curProductCategory)
        {
            var emptyResult = new Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>>();
            var criteriaContaines = curProductCategory.CriteriaCollection?.FilteredItems.Select(x => x.ContentLink.ID);
            if (criteriaContaines.IsNullOrEmpty()) return emptyResult;

            criteriaContaines = criteriaContaines.Take(MaxCriteriaNumber);

            var allFamilies = GetAllFamily(curProductCategory.ContentLink.ID);
            var criteriaIds = GetDistinctedCriteriaIds(allFamilies);
            if (criteriaIds.IsNullOrEmpty()) return emptyResult;

            return GetCriteriasForSearching(criteriaContaines, criteriaIds);
        }

        private Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>> GetCriteriasForSearching(IEnumerable<int> criteriaContaines,
            IEnumerable<int> criteriaIds)
        {
            if (criteriaContaines.IsNullOrEmpty() || criteriaIds.IsNullOrEmpty())
            {
                return new Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>>();
            }
            var filterExpression = new FilterExpression<IProductFamilyProperty>(pf => ((IContent)pf).ContentLink.ID.In(criteriaIds)
                                                                                & ((IContent)pf).ParentLink.ID.In(criteriaContaines));
            var criteriasForSearching = PageService.GetContents(FindSettings.MaxItemsPerRequest, filterExpression.Expression);
            var groupedCriteria = from criteria in criteriasForSearching
                                  group criteria by ((IContent)criteria).ParentLink.ID into newGroup
                                  select newGroup;
            var result = new Dictionary<CriteriaContainerPage, IEnumerable<IProductFamilyProperty>>();
            foreach (var criteriaContainerId in criteriaContaines)
            {
                var criteria = groupedCriteria.FirstOrDefault(g => g.Key == criteriaContainerId);
                if (criteria == null) continue;
                CriteriaContainerPage containerPage;
                if (!ContentLoader.TryGet(new ContentReference(criteriaContainerId), out containerPage)) continue;
                result.Add(containerPage, criteria);
            }
            return result;
        }

        private IEnumerable<int> GetDistinctedCriteriaIds(IEnumerable<ProductFamilyPage> allFamilies)
        {
            var allPropertyIdCollections = allFamilies?.Select(f => f.PropertyIdCollection());
            var allCriteriaIds = new List<int>();
            foreach (var item in allPropertyIdCollections)
            {
                allCriteriaIds.AddRange(item);
            }

            return allCriteriaIds.Distinct();
        }

        private IEnumerable<ProductFamilyPage> GetAllFamily(int categoryId)
        {
            var filterExpression = new FilterExpression<ProductFamilyPage>(pf => pf.ProductCategoryIdCollection().Match(categoryId));
            return PageService.GetPages(FindSettings.MaxItemsPerRequest, filterExpression.Expression);
        }

        public IEnumerable<string> GetCriteriaTypesDisplayInHeader(int[] criteriaTypeIds)
        {
            if (criteriaTypeIds.AsEnumerable().IsNullOrEmpty()) return null;
            var filterBuilder = ClientSearch.BuildFilter<CriteriaContainerPage>()
                .And(m => m.ContentLink.ID.In(criteriaTypeIds))
                .And(m => m.DisplayInFamilyListHeader.Match(true));

            var filterExpression = new FilterExpression<CriteriaContainerPage>(m => filterBuilder);
            var result = PageService.GetPages(FindSettings.MaxItemsPerRequest, filterExpression.Expression);
            return result.IsNullOrEmpty() ? null : result.Select(x => x.Name);
        }

        public IEnumerable<ProductFamilyPage> GetAllProductFamilyByCategoryPage(ProductCategoryPage curProductCategory)
        {
            return GetAllFamily(curProductCategory.ContentLink.ID);
        }
    }
}