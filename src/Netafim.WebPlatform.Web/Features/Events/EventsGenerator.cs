using Castle.Core.Internal;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Events;
using Netafim.WebPlatform.Web.Features.Home;
using System;

namespace Netafim.WebPlatform.Web.Features.Events
{
    public class EventsGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;
        public EventsGenerator(IContentRepository contentRepo, IUrlSegmentCreator urlSegment, ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepo;
            _urlSegmentCreator = urlSegment;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var homepage = _contentRepository.Get<HomePage>(context.Homepage);
            if (homepage == null) { return; }

            EnsureEventData(context);
        }

        private void EnsureEventData(ContentContext context)
        {
            var eventPages = _contentRepository.GetChildren<EventPage>(context.Homepage);
            if (eventPages != null && !eventPages.IsNullOrEmpty()) return;

            for (int i = 0; i < 5; i++)
            {
                var page = _contentRepository.GetDefault<EventPage>(context.Homepage).CreateWritableClone() as EventPage;
                page.From = DateTime.Now.AddDays(i + 1);
                page.To = DateTime.Now.AddDays(i + i + 1);

                page.Title = string.Format("Event {0}", i == 0 ? string.Empty : i.ToString());
                page.PageName = page.Title;
                page.Location = "Ha Noi, Viet Nam";
                page.EventLink = new Url("http://google.com");
                page.LinkText = "Event Link";

                page.URLSegment = _urlSegmentCreator.Create(page);
                _contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess);

            }

            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(context.Homepage);
            var eventListingBlock = _contentRepository.GetDefault<EventListingBlock>(assetsFolder.ContentLink).CreateWritableClone() as EventListingBlock;
            ((IContent)eventListingBlock).Name = "Event Overview";
            eventListingBlock.Title = "Event";
           var eventListingBlockRef= _contentRepository.Save((IContent)eventListingBlock, SaveAction.Publish, AccessLevel.NoAccess);

            var eventOverviewPage = _contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            eventOverviewPage.PageName = "Event overview";
            eventOverviewPage.Name = "Event overview";            
            eventOverviewPage.Content = new ContentArea();
            eventOverviewPage.Content.Items.Add(new ContentAreaItem() { ContentLink = eventListingBlockRef });

            _contentRepository.Save(eventOverviewPage, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}