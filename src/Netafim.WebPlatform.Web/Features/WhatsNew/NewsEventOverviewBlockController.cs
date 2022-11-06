using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.Events;
using Netafim.WebPlatform.Web.Features.NewsOverview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    public class NewsEventOverviewBlockController : BlockController<NewsEventOverviewBlock>
    {
        private readonly INewsRepository _newsRepo;
        private readonly IEventsReopsitory _eventRepo;
        private  UrlHelper _urlHelper;
        private readonly IEnumerable<IEventTimeRangeDisplay> _eventTimeRangeDisplays;
        public NewsEventOverviewBlockController(INewsRepository newsRepo, IEventsReopsitory eventRepo, IEnumerable<IEventTimeRangeDisplay> eventTimeRangeDisplays)
        {
            _newsRepo = newsRepo;
            _eventRepo = eventRepo;
            _eventTimeRangeDisplays = eventTimeRangeDisplays;
            
        }
        public override ActionResult Index(NewsEventOverviewBlock currentBlock)
        {
            var viewModel = new NewsEventOverviewBlockModel(currentBlock)
            {
                Items = GetNewsEventItems(currentBlock)
            };
            _urlHelper = new UrlHelper(Request.RequestContext);
            return PartialView(currentBlock.GetDefaultFullViewName(), viewModel);
        }

        private IEnumerable<NewsEventItemViewModel> GetNewsEventItems(NewsEventOverviewBlock currentBlock)
        {
            var latestNews = _newsRepo.GetLatestNewsPages(currentBlock.TotalNewsItems);
            var upCommingEvents = _eventRepo.GetUpcommingEvents(currentBlock.TotalEventItems);
            latestNews = latestNews ?? Enumerable.Empty<NewsPage>();
            upCommingEvents = upCommingEvents ?? Enumerable.Empty<EventPage>();
            return latestNews.Select(x => MapToNewsEventItemModel(x)).Union(upCommingEvents.Select(x => MapToNewsEventItemModel(x)));
        }

        private NewsEventItemViewModel MapToNewsEventItemModel(EventPage eventPage)
        {
            return new NewsEventItemViewModel()
            {
                Image = eventPage.Image,
                PostLink = _urlHelper.ContentUrl(eventPage.EventLink),
                PostDate = eventPage.StartPublish.Value,
                Description= $"{eventPage.Title} \n {GetDateTimeRange(eventPage.From, eventPage.To)}.{eventPage.Location}"
            };
        }

        private string GetDateTimeRange(DateTime from, DateTime to)
        {
            var eventTimeDisplay = _eventTimeRangeDisplays.FirstOrDefault(x => x.Satified(from, to));
            if (eventTimeDisplay == null) throw new Exception("Cannot find suitable service for display time range of event.");
            return eventTimeDisplay.ToDateTimeRange(from, to);
        }

        private NewsEventItemViewModel MapToNewsEventItemModel(NewsPage newsPage)
        {
            return new NewsEventItemViewModel()
            {
                Description = newsPage.Title,
                Image = newsPage.Image,
                PostDate = newsPage.NewsDate,
                PostLink = _urlHelper.ContentUrl(newsPage.ContentLink)
            };
        }
    }
}