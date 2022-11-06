using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Search;

namespace Netafim.WebPlatform.Web.Core.Facet
{
    public static class FacetExtensions
    {
        public static string[] CategoriesFacet(this ICanBeSearched item)
        {
            var searchResultPage = item as PageData;

            if (searchResultPage == null || searchResultPage.Category == null) return null;

            if (string.IsNullOrWhiteSpace(searchResultPage.Category.ToString()))
            {
                return null;
            }

            return searchResultPage.Category.ToString().Split(',');
        }
    }
}