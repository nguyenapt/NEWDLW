using Castle.Core.Internal;
using EPiServer;
using EPiServer.Find;
using EPiServer.Find.Api;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.JobDetails;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Find;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public class JobFilterQueryComposer : QueryComposerBase<JobFilterBlock, JobFilterBlockQueryViewModel>
    {
        private readonly IClient _searchClient;
        public JobFilterQueryComposer(IContentLoader contentLoader, IClient client) : base(contentLoader)
        {
            _searchClient = client;
        }

        public override FilterExpression<ICanBeSearched> Compose(QueryViewModel query)
        {
            var jobFilterQuery = GetQueryModel(query);          

            var filterBuilder = _searchClient.BuildFilter<ICanBeSearched>();
            if (!jobFilterQuery.Department.IsNullOrEmpty())
            {
                filterBuilder = filterBuilder.And(m => ((JobDetailsPage)m).Department.Match(jobFilterQuery.Department));
            }
            if (!jobFilterQuery.Location.IsNullOrEmpty())
            {
                filterBuilder = filterBuilder.And(m => ((JobDetailsPage)m).Location.Match(jobFilterQuery.Location));
            }

            filterBuilder = filterBuilder.And(m => m.MatchType(typeof(JobDetailsPage)));

            return new FilterExpression<ICanBeSearched>(m => filterBuilder);
        }
        public override Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder> GetSortings(QueryViewModel query)
        {
            var sortings = new Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder>
            {
                { m => ((JobDetailsPage)m).Position, SortOrder.Ascending },
                { m => ((JobDetailsPage)m).Department, SortOrder.Ascending },
                { m => ((JobDetailsPage)m).Location, SortOrder.Ascending }
            };
            return sortings;
        }
    }
}