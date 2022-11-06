using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    [ContentType(DisplayName = "News Event Overview Block",
        GroupName = GroupNames.Overview,
        Description = "Component that display the list of news and events.",
        GUID = "4bc5b747-c20f-4479-b9f4-68c8257eaf1c")]
    public class NewsEventOverviewBlock : ItemBaseBlock
    {
        [CultureSpecific]
        public virtual string Title { get; set; }

        [Display(Name = "Event Overview Page")]
        public virtual ContentReference EventOverviewPage { get; set; }

        [Display(Name = "News Overview Page")]
        public virtual ContentReference NewsOverviewPage { get; set; }

        [Display(Name = "Total News Items")]
        [Range(1, 5)]
        public virtual int TotalNewsItems { get; set; }
        
        [Display(Name = "Total Event Items")]
        [Range(1, 5)]
        public virtual int TotalEventItems { get; set; }
    }
}