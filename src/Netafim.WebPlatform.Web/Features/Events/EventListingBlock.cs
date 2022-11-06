using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.Events
{
    [ContentType(DisplayName = "Event Listing Block", GroupName = GroupNames.Overview,
        Description = "Component that display the list of upcomming events.")]
    public class EventListingBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20, Name = "Total Events To Display", Description ="Specify the total events to display(maximum is 1000)")]
        [Range(0, 1000)]
        public virtual int TotalNewsToDisplay { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 30, Name = "Events Overview Page")]
        public virtual ContentReference EventsOverviewPage { get; set; }

    }
}