using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.Filters;
using Castle.Core.Internal;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class IContentAreaExtensions
    {
        public static IEnumerable<IContent> GetFilteredItemsContent(this ContentArea contentArea, int? take = null, int? skip = null)
        {
            if (contentArea != null)
            {
                var filteredItems = contentArea.FilteredItems;
                if (filteredItems != null && filteredItems.Any())
                {
                    var filterPublished = new FilterPublished(PagePublishedStatus.Published);

                    var filteredItemsContent = filteredItems
                        .Skip(skip ?? 0)
                        .Take(take ?? filteredItems.Count())
                        .Select(a => a.GetContent())
                        .Where(a => a != null)
                        .ToList();

                    filterPublished.Filter(filteredItemsContent);

                    return filteredItemsContent;
                }
            }
            return Enumerable.Empty<IContent>();
        }

        public static IEnumerable<T> GetFilteredItemsContent<T>(this ContentArea contentArea, int? take = null, int? skip = null) where T : IContentData
        {
            return GetFilteredItemsContent(contentArea, take, skip).OfType<T>();
        }

        public static bool IsNullOrEmpty(this ContentArea contentArea) 
        {
            return contentArea == null || contentArea.FilteredItems == null || contentArea.FilteredItems.IsNullOrEmpty();
        }

        public static bool IsNullOrEmptyForViewing(this ContentArea contentArea)
        {
            return contentArea == null || contentArea.FilteredItems == null || contentArea.FilteredItems.IsNullOrEmpty();
        }
    }
}