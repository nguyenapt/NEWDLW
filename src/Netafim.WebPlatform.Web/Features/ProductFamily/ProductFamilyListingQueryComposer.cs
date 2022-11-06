using Castle.Core.Internal;
using EPiServer;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Find;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using System;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public class ProductFamilyListingQueryComposer : QueryComposerBase<FamilyMatrixBlock, FamilyMatrixQueryViewModel>
    {
        private readonly IClient _searchClient;
        public ProductFamilyListingQueryComposer(IContentLoader contentLoader, IClient client) : base(contentLoader)
        {
            _searchClient = client;
        }

        public override FilterExpression<ICanBeSearched> Compose(QueryViewModel query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var familyQuery = GetQueryModel(query);

            if (familyQuery == null)
                throw new ArgumentException($"The expected type of query parameter is {nameof(FamilyMatrixQueryViewModel)}");

            var filterBuilder = _searchClient.BuildFilter<ICanBeSearched>();
            if (!familyQuery.Criteria.AsEnumerable().IsNullOrEmpty())
            {
                foreach (var criteria in familyQuery.Criteria)
                {
                    if(criteria != Constants.SelectAllValue)
                    {
                        filterBuilder = filterBuilder.And(m => ((ProductFamilyPage)m).PropertyIdCollection().Match(criteria));
                    }
                }
            }

            filterBuilder = filterBuilder.And(m => m.MatchType(typeof(ProductFamilyPage)));
            if (familyQuery.ProductCategoryId > 0)
            {
                filterBuilder = filterBuilder.And(m => ((ProductFamilyPage)m).ProductCategoryIdCollection().Match(familyQuery.ProductCategoryId));
            }         
            return new FilterExpression<ICanBeSearched>(m => filterBuilder);
        }
    }
}