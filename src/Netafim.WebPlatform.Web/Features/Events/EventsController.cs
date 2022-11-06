using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.Events
{
    public class EventsController : BlockController<EventListingBlock>
    {
        private readonly IEventsReopsitory _eventRepo;
        public EventsController(IEventsReopsitory eventRepo)
        {
            _eventRepo = eventRepo;
        }
        public override ActionResult Index(EventListingBlock currentContent)
        {
            var viewModel = new EventListingModel(currentContent)
            {
                Events = _eventRepo.GetUpcommingEvents(currentContent.TotalNewsToDisplay)
            };
            return PartialView("EventListingBlock", viewModel);
        }
    }
}