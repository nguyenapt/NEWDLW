using Dlw.EpiBase.Content.Cms.Search;
using EPiServer.Find;
using EPiServer.Find.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Netafim.WebPlatform.Web.Features.Events
{
    public class EventsRepository : IEventsReopsitory
    {
        private readonly IPageService _pageService;

        public EventsRepository(IPageService pageService)
        {
            _pageService = pageService;
        }

        public IEnumerable<EventPage> GetUpcommingEvents(int totalItemsToGet)
        {
            if (totalItemsToGet <= 0) return Enumerable.Empty<EventPage>();
            var filter = CreateSearchFilter();
            var sorting = new Dictionary<Expression<Func<EventPage, DateTime>>, SortOrder>
            {
                { ev => ev.From, SortOrder.Ascending }
            };
            return _pageService.GetPagesWithSorting(totalItemsToGet, filter.Expression, sorting);
        }

        private FilterExpression<EventPage> CreateSearchFilter()
        {
            return new FilterExpression<EventPage>(ep => ep.From.GreaterThan(DateTime.Now));
        }
    }
}