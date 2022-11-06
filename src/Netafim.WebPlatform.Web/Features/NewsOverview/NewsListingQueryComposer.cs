using EPiServer;
using EPiServer.Find;
using EPiServer.Find.Api;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Find;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Netafim.WebPlatform.Web.Features.NewsOverview
{
    public class NewsListingQueryComposer : QueryComposerBase<NewsOverviewBlock, NewsOverviewQueryViewModel>
    {
        private readonly IClient _searchClient;
        public NewsListingQueryComposer(IContentLoader contentLoader, IClient client) : base(contentLoader)
        {
            _searchClient = client;
        }
        public override FilterExpression<ICanBeSearched> Compose(QueryViewModel query)
        {
            var newsListingQuery = GetQueryModel(query) as NewsOverviewQueryViewModel;
            if (newsListingQuery == null)
                throw new ArgumentException($"The expected type of query parameter is {nameof(NewsOverviewQueryViewModel)}");
            var filterBuilder = _searchClient.BuildFilter<ICanBeSearched>();
            filterBuilder = filterBuilder.And(m => m.MatchType(typeof(NewsPage)));

            var year = newsListingQuery.Year;
            if (year != DateTime.MinValue.Year && year != 0)
            {
                filterBuilder = filterBuilder.And(m => ((NewsPage)m).NewsDate.MatchYear(year));
            }
            return new FilterExpression<ICanBeSearched>(m => filterBuilder);
        }
        public override Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder> GetSortings(QueryViewModel query)
        {
            var sortings = new Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder>
            {
                { m => ((NewsPage)m).NewsDate, SortOrder.Descending }                
            };
            return sortings;
        }
    }
}