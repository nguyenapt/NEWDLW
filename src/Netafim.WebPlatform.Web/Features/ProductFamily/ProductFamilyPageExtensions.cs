using Castle.Core.Internal;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public static class ProductFamilyPageExtensions
    {
        public static IEnumerable<T> GetSelectedProperties<T>(this ProductFamilyPage page, IEnumerable<int> criteriaContainerIds, IEnumerable<int> criteriaIds) where T : IProductFamilyProperty
        {
            var contentItems = page.PropertyCollection.GetFilteredItemsContent<IProductFamilyProperty>();
            if (contentItems.IsNullOrEmpty() || criteriaContainerIds.AsEnumerable().IsNullOrEmpty() || criteriaIds.IsNullOrEmpty())
            {
                return contentItems?.Cast<T>();
            }

            var result = new List<T>();
            foreach (var containerId in criteriaContainerIds)
            {
                var children = contentItems.Where(p => ((IContent)p).ParentLink.ID == containerId);
                if (children == null) continue;
                var propertyPage = children.Count() <= 1 || !(children.Any(p => criteriaIds.Contains(((IContent)p).ContentLink.ID)))
                                                        ? children.FirstOrDefault()
                                                        : children.FirstOrDefault(p => criteriaIds.Contains(((IContent)p).ContentLink.ID));

                if (propertyPage != null)
                {
                    result.Add((T)propertyPage);
                }
            }
            return result;
        }
    }
}