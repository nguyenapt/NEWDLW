using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.NewsEventOverview
{
    [ContentType(DisplayName = "News Listing Block",
        GroupName = GroupNames.Overview,
        Description = "Component that display the list of upcomming events.",
        GUID = "8eca56eb-d73e-435e-9b49-a76ad3a0d8b7")]
    public class NewsListingBlock : ListingBaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20, Name = "Maximum News To Display")]
        [Range(1, 100)]
        public virtual int MaximumNewsToDisplay { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 30, Name = "NewsOverview Page")]
        public virtual ContentReference NewsOverviewPage { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            MaximumNewsToDisplay = 3;
        }
    }
}