using System.Collections.Generic;
using EPiServer.Forms.Core.Feed.Internal;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Core.Provider
{
    public class FeedProvider : IFeedProvider
    {
        public IEnumerable<IFeed> GetFeeds()
        {
            return ServiceLocator.Current.GetAllInstances<IFeed>();
        }
    }
}