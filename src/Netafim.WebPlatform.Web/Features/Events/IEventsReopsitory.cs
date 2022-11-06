using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Events
{
    public interface IEventsReopsitory
    {
        IEnumerable<EventPage> GetUpcommingEvents(int totalItemsToGet);

    }
}