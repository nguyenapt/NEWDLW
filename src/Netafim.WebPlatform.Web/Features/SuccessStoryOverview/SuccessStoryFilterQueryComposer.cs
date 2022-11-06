using System;
using Castle.Core.Internal;
using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.SuccessStory;
using Netafim.WebPlatform.Web.Features.SuccessStoryOverview.ViewModels;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Find;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview
{
    public class SuccessStoryFilterQueryComposer : QueryComposerBase<SuccessStoryFilterBlock, SuccessStoryQueryViewModel>
    {
        private readonly IClient _searchClient;
        private readonly CategoryRepository _categoryRepository;
        public SuccessStoryFilterQueryComposer(IContentLoader contentLoader, IClient client, CategoryRepository categoryRepository) : base(contentLoader)
        {
            _searchClient = client;
            _categoryRepository = categoryRepository;
        }

        public override FilterExpression<ICanBeSearched> Compose(QueryViewModel query)
        {
            var filterQuery = GetQueryModel(query);
            var filterBuilder = _searchClient.BuildFilter<ICanBeSearched>();
            if (filterQuery.CropId > 0)
            {
                filterBuilder = filterBuilder.And(m => ((SuccessStoryPage) m).CropId.Match(filterQuery.CropId));
            }
            if (!filterQuery.Country.IsNullOrEmpty())
            {
                filterBuilder = filterBuilder.And(m => ((SuccessStoryPage)m).Country.Match(filterQuery.Country));
            }
            if (filterQuery.BigProject)
            {
                var isBigProject = _categoryRepository.Get(typeof(Big).Name);
                if (isBigProject != null)
                {
                    filterBuilder = filterBuilder.And(m => ((SuccessStoryPage)m).Category.Match(isBigProject.ID));
                }
            }
            if (filterQuery.FilterWithBoostedDate)
            {
                filterBuilder = filterBuilder.And(m => ((SuccessStoryPage) m).BoostedTo.GreaterThan(DateTime.Today))
                    .Or(m => ((SuccessStoryPage) m).BoostedTo.Match(DateTime.Today));
            }
            else
            {
                filterBuilder = filterBuilder.And(m => ((SuccessStoryPage) m).BoostedTo.LessThan(DateTime.Today));
            }

            filterBuilder = filterBuilder.And(m => m.MatchType(typeof (SuccessStoryPage)));

            return new FilterExpression<ICanBeSearched>(m => filterBuilder);
        }
    }
}