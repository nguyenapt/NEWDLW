using Dlw.EpiBase.Content.Cms.Search;
using EPiServer.Find;
using EPiServer.Find.Api;
using Netafim.WebPlatform.Web.Core.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Netafim.WebPlatform.Web.Features.NewsOverview
{
    public class NewsRepository : INewsRepository
    {
        private readonly IPageService _pageService;
        private readonly IFindSettings _findSettings;
        protected readonly IClient _searchClient;
        public NewsRepository(IPageService pageService, IFindSettings findSettings, IClient client)
        {
            _pageService = pageService;
            _findSettings = findSettings;
            _searchClient = client;
        }


        private FilterExpression<ICanBeSearched> GetYearNewsStatisticFilter(int year)
        {
            var filterBuilder = _searchClient.BuildFilter<ICanBeSearched>();
            filterBuilder = filterBuilder.And(m => m.MatchType(typeof(NewsPage)));
            filterBuilder = filterBuilder.And(m => ((NewsPage)m).NewsDate.MatchYear(year));

            return new FilterExpression<ICanBeSearched>(m => filterBuilder);
        }

        public int GetLatestYearHavingNews(IEnumerable<int> yearsRange)
        {
            foreach (var year in yearsRange)
            {
                var newsPages = _pageService.GetContents<ICanBeSearched>(1, GetYearNewsStatisticFilter(year).Expression);
                if (newsPages != null && newsPages.TotalMatching > 0) { return year; }
            }
            return DateTime.Now.Year;
        }

        public IEnumerable<NewsPage> GetLatestNewsPages(int maximumNewsToDisplay)
        {
            FilterExpression<NewsPage> filter = GetNewsFilter();
            if (maximumNewsToDisplay == 0) { return Enumerable.Empty<NewsPage>(); }

            var sortings = new Dictionary<Expression<Func<NewsPage, DateTime>>, SortOrder>
            {
                { m => m.NewsDate, SortOrder.Descending }
            };
            return _pageService.GetPagesWithSorting(maximumNewsToDisplay, filter.Expression, sortings);
        }

        private FilterExpression<NewsPage> GetNewsFilter()
        {
            var filterBuilder = _searchClient.BuildFilter<NewsPage>();
            filterBuilder = filterBuilder.And(m => m.MatchType(typeof(NewsPage)));
            return new FilterExpression<NewsPage>(m => filterBuilder);
        }
    }
}