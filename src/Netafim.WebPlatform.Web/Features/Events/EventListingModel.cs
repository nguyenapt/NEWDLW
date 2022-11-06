
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Events
{
    public class EventListingModel
    {
        public EventListingBlock Block { get; set; }
        public IEnumerable<EventPage> Events { get; set; }

        public readonly int ItemsEachRow = 3;

        public EventListingModel(EventListingBlock block)
        {
            this.Block = block;            
        }
    }
}