using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using EPiServer.Forms.Core;
using EPiServer.Forms.Core.Feed.Internal;
using EPiServer.Forms.EditView.Models.Internal;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.JobFilter;

namespace Netafim.WebPlatform.Web.Features.JobApplicationForm
{
    [ServiceConfiguration(ServiceType = typeof(IFeed))]
    public class JobPositionFeed : IFeed, IUIEntityInEditView
    {
        public IEnumerable<IFeedItem> LoadItems()
        {
            var jobFilterSettings = ServiceLocator.Current.GetInstance<IJobFilterSettings>();
            var allPositions = jobFilterSettings.JobPositions;
            if (allPositions.IsNullOrEmpty())
                return Enumerable.Empty<IFeedItem>();

            return allPositions.Select(t => new FeedItem
            {
                Key = t.JobName,
                Value = t.JobName
            });
        }

        public string ID => "fac8134e-117b-4e4a-8313-273dccdaddec";
        public string Description { get { return "Job Position Feed"; } set {} }
        public string ExtraConfiguration => string.Empty;
        public string EditViewFriendlyTitle => Description;
        public bool AvailableInEditView => true;
    }
}