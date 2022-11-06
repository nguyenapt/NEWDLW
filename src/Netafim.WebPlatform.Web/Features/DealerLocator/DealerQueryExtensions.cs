using EPiServer.Find;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Api.Querying.Queries;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{

    public static class DealerQueryExtensions
    {
        public static DelegateFilterBuilder Wildcard(this string value, string term)
        {
            return new DelegateFilterBuilder(field => new QueryFilter(new WildcardQuery(field, $"*{term}*")));
        }
    }
}