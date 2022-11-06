using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api;
using EPiServer.Find.Cms;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.CropsOverview;
using Netafim.WebPlatform.Web.Features.SuccessStory;
using Netafim.WebPlatform.Web.Features.SuccessStoryOverview.Repository;
using Netafim.WebPlatform.Web.Features.SuccessStoryOverview.ViewModels;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;
using Global = Netafim.WebPlatform.Web.Core.Templates.Global;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview
{
    public class SuccessStoryOverviewController : ListingBaseBlockController<SuccessStoryFilterBlock, SuccessStoryQueryViewModel, SuccessStoryPage>
    {
        private readonly ISuccessStoryFilterRepository _successStoryFilterRepository;
        public SuccessStoryOverviewController(IContentLoader contentLoader,
            IPageService pageService,
            IFindSettings findSettings,
            ISuccessStoryFilterRepository successStoryFilterRepository) : base(contentLoader, pageService, findSettings)
        {
            _successStoryFilterRepository = successStoryFilterRepository;
        }
        public override ActionResult Index(SuccessStoryFilterBlock currentBlock)
        {
            var model = new SuccessStoryFilterViewModel(currentBlock)
            {
                Crops = _successStoryFilterRepository.Crops(),
                Countries = _successStoryFilterRepository.Countries()
            };
            return PartialView(string.Format(Global.Constants.AbsoluteViewPathFormat, "SuccessStoryOverview", "_successStoryFilter"), model);
        }

        protected override ActionResult PopulateView(SuccessStoryQueryViewModel query)
        {
            var block = ContentLoader.Get<SuccessStoryFilterBlock>(new ContentReference(query.BlockId));
            var currentPage = query.HasHashData ? query.CurrentPage : query.CurrentPage - 1;
            var take = query.HasHashData ? (currentPage * block.PageSize) : block.PageSize;
            var skip = query.HasHashData ? 0 : (currentPage * block.PageSize);

            var boostedResult = MakeQuery(query, skip, take, true);
            IEnumerable<ICanBeSearched> finalResult;
            IContentResult<ICanBeSearched> restOfResult;
            int totalMatching;
            if (boostedResult.TotalMatching > 0)
            {
                var resultCaculated = SuccessStoryOverviewExtensions.CalculateTakeAndSkipItem(boostedResult.Items.Count(), boostedResult.TotalMatching, block.PageSize, currentPage, query.HasHashData);

                restOfResult = MakeQuery(query, resultCaculated.Item1, resultCaculated.Item2, false);
                totalMatching = CalculateTotalMatching(boostedResult, restOfResult);
                finalResult = boostedResult.Items.Union(restOfResult.Items);
            }
            else
            {
                restOfResult = MakeQuery(query, skip, take, false);
                totalMatching = restOfResult.TotalMatching;
                finalResult = restOfResult;
            }

            var pagedList = new PagedList<SuccessStoryPage>(finalResult.Cast<SuccessStoryPage>(), totalMatching, block.PageSize, currentPage);

            var cropPages = PageService.GetPages<CropsPage>(FindSettings.MaxItemsPerRequest, m => m.MatchType(typeof(CropsPage)));

            var viewModel = new SuccessStoryListingViewModel(block, pagedList, cropPages);
            var htmlContent = this.RenderViewToString(string.Format(Global.Constants.AbsoluteViewPathFormat, "SuccessStoryOverview", "_successStoryResultFilter"), viewModel);
            var data = new
            {
                html = htmlContent,
                totalsResult = pagedList.TotalResult,
                totalPage = pagedList.TotalPage
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private IContentResult<ICanBeSearched> MakeQuery(SuccessStoryQueryViewModel query, int skip, int take,
            bool filterWithBoostedDate)
        {
            query.FilterWithBoostedDate = filterWithBoostedDate;
            var composer = GetQueryComposer(query);
            var result = PageService.GetContentsWithSorting(take, composer.Compose(query)?.Expression, GetSortings(filterWithBoostedDate), skip);

            return result;
        }

        private static Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder> GetSortings(bool filterWithBoostedDate)
        {
            var sortings = new Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder>();
            if (filterWithBoostedDate)
            {
                sortings.Add(m => ((SuccessStoryPage) m).BoostedFrom, SortOrder.Ascending);
            }
            else
            {
                sortings.Add(m => ((SuccessStoryPage)m).ProjectDate, SortOrder.Descending);
            }

            return sortings;
        }

        private int CalculateTotalMatching(IContentResult<ICanBeSearched> boostedResult,
            IContentResult<ICanBeSearched> restOfResult)
        {
            return boostedResult.TotalMatching + restOfResult.TotalMatching;
        }
    }
}