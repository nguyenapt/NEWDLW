using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.NewsOverview
{
    [ContentType(DisplayName = "News Overview Block",
        GroupName = GroupNames.Overview,
        Description = "Component that display the list of upcomming events.",
        GUID = "40333575-62a5-4ad5-b3ff-d9e1adb54619")]
    public class NewsOverviewBlock : ListingBaseBlock, IComponent
    {
        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName();

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10, Name = "Total Years To Display News",
            Description = "Total years from the current year backfoward to display news")]
        [Range(1, 10)]
        public virtual int TotalYearsToDisplayNews { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            TotalYearsToDisplayNews = 5;
        }
    }
}