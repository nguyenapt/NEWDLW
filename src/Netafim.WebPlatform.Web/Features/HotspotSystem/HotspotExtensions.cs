using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    public static class HotspotExtensions
    {
        public static IEnumerable<T> GetHotspotNodes<T>(this IHotspotTemplate hotspotTemplate) where T : IContentData
        {
            if (hotspotTemplate.ContentLink == ContentReference.EmptyReference || hotspotTemplate.ContentLink == null || hotspotTemplate.ContentLink.ID == 0)
            {
                return Enumerable.Empty<T>();
            }

            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

            return contentRepository.GetChildren<T>(hotspotTemplate.ContentLink).ToList();
        }

        public static string ToHotspotNodeId(this HotspotLinkNodePage node, string prefixNodeId)
        {
            return $"{prefixNodeId}_{node.CoordinatesX}_{node.CoordinatesY}";
        }
    }
}