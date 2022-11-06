using Castle.Core.Internal;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Filters;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Events;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.MediaCarousel;
using Netafim.WebPlatform.Web.Features.NewsEventOverview;
using System;

namespace Netafim.WebPlatform.Web.Features.NewsOverview
{
    public class NewsOverviewContentGenerator : IContentGenerator
    {
        private const string DataDemoFolder = @"~/Features/NewsOverview/Data/Demo/{0}";

        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        private ILogger _logger = LogManager.GetLogger();
        public NewsOverviewContentGenerator(IContentRepository contentRepository,
            IUrlSegmentCreator urlSegmentCreator,
            ContentAssetHelper contentAssetHelper
            )
        {
            _contentRepository = contentRepository;
            _urlSegmentCreator = urlSegmentCreator;
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

            EnsureNewsData(context);

        }

        private void EnsureNewsData(ContentContext context)
        {
            if (DoesExistNewsPage(context)) return;

            var newsOverviewPage = CreateNewsOverviewContainerPage(context);
            if (newsOverviewPage == null) return;

            var newsTitles = new[] { "What is Lorem Ipsum?", "Why do we use it?", "Where does it come from?", "Where can I get some?" };
            for (int i = 0; i < newsTitles.Length; i++)
            {
                var newsPage = InitPage<NewsPage>(newsOverviewPage, newsTitles[i]);
                newsPage.Image = newsPage.CreateBlob(string.Format(DataDemoFolder, "620x452.jpg"));
                if (i == 0)
                {
                    newsPage.NewsDate = DateTime.Now.AddYears(-1).AddDays(-1);
                }
                Save(newsPage);
                _logger.Error("NewsOverviewContentGenerator,newsPage: " + newsPage);
            }

            CreateNewsEventsOverviewData(context);
        }

        private void CreateNewsEventsOverviewData(ContentContext context)
        {
            var newsEventsOverviewPage = InitPage<GenericContainerPage>(context.Homepage, "News and Events overview");
            var savedOverviewPage = Save(newsEventsOverviewPage);

            var editableOverviewPage = newsEventsOverviewPage.CreateWritableClone() as GenericContainerPage;
            var savedHeroComponent = CreateHeroNewsComponent(savedOverviewPage);
            var savedNewsListingBlock = CreateNewsListingComponent(savedOverviewPage);
            var savedEventsListingBlock = CreateEventsListingComponent(savedOverviewPage);

            editableOverviewPage.BannerArea = new ContentArea();
            editableOverviewPage.BannerArea.Items.Add(new ContentAreaItem() { ContentLink = savedHeroComponent });

            editableOverviewPage.Content = new ContentArea();
            editableOverviewPage.Content.Items.Add(new ContentAreaItem() { ContentLink = savedNewsListingBlock });
            editableOverviewPage.Content.Items.Add(new ContentAreaItem() { ContentLink = savedEventsListingBlock });

            Save(editableOverviewPage);
        }

        private ContentReference CreateEventsListingComponent(ContentReference savedOverviewPage)
        {
            var eventListingBlock = InitBlock<EventListingBlock>(_contentAssetHelper.GetOrCreateAssetFolder(savedOverviewPage), "Events Listing");
            eventListingBlock.TotalNewsToDisplay = 6;
            eventListingBlock.Title = "Events";
            eventListingBlock.EventsOverviewPage = savedOverviewPage;
            return Save((IContent)eventListingBlock);
        }

        private ContentReference CreateNewsListingComponent(ContentReference savedOverviewPage)
        {
            var newsListingBlock = InitBlock<NewsListingBlock>(_contentAssetHelper.GetOrCreateAssetFolder(savedOverviewPage), "News Listing");
            newsListingBlock.MaximumNewsToDisplay = 6;
            newsListingBlock.Title = "News";
            newsListingBlock.NewsOverviewPage = ContentReference.StartPage;
            return Save((IContent)newsListingBlock);
        }

        private ContentReference CreateHeroNewsComponent(ContentReference savedOverviewPage)
        {
            var imgCarouselBlock = InitBlock<ImageCarouselBlock>(_contentAssetHelper.GetOrCreateAssetFolder(savedOverviewPage), "News Hero");
            imgCarouselBlock.Text = new XhtmlString("<p>Our new mobile app combines field, climate and weather data to forecast the watering needs of corn crops. Our expertise supports you to grow even more.</p>");
            imgCarouselBlock.Link = new Url("http://google.com");
            imgCarouselBlock.Title = "News";
            imgCarouselBlock.Image = ((IContent)imgCarouselBlock).CreateBlob(string.Format(DataDemoFolder, "1440x620.png"));
            var savedImgCarouselBlock= Save((IContent)imgCarouselBlock);

            var imgCarouselContainer= InitBlock<MediaCarouselContainerBlock>(_contentAssetHelper.GetOrCreateAssetFolder(savedOverviewPage), "News Hero Container");
            imgCarouselContainer.Items = new ContentArea();
            imgCarouselContainer.Items.Items.Add(new ContentAreaItem() {ContentLink= savedImgCarouselBlock });
            return Save((IContent)imgCarouselContainer);
        }

        private ContentReference CreateNewsOverviewContainerPage(ContentContext context)
        {
            var page = InitPage<GenericContainerPage>(context.Homepage, "News overview");
            var savedPage = Save(page);

            var editablePage = page.CreateWritableClone() as GenericContainerPage;
            var newsOverviewBlock = InitBlock<NewsOverviewBlock>(_contentAssetHelper.GetOrCreateAssetFolder(savedPage), "News Overview");
            newsOverviewBlock.TotalYearsToDisplayNews = 5;
            var savedBlock = Save((IContent)newsOverviewBlock);
            _logger.Error("NewsOverviewContentGenerator,newsOverviewBlock: " + newsOverviewBlock);
            //editablePage.AddItemsToMainContent(new[] { savedBlock });
            editablePage.Content = new ContentArea();
            editablePage.Content.Items.Add(new ContentAreaItem() { ContentLink = savedBlock });

            return Save(editablePage);
        }

        private bool DoesExistNewsPage(ContentContext context)
        {
            var pageTypeId = ServiceLocator.Current.GetInstance<IContentTypeRepository>().Load<NewsPage>().ID.ToString();

            var criteria = new PropertyCriteria
            {
                Condition = CompareCondition.Equal,
                Name = "PageTypeID",
                Type = PropertyDataType.PageType,
                Value = pageTypeId,
                Required = true
            };
            var pages = DataFactory.Instance.FindPagesWithCriteria((PageReference)context.Homepage, new PropertyCriteriaCollection() { criteria });
            return !pages.IsNullOrEmpty();
        }

        private T InitBlock<T>(ContentAssetFolder folder, string name) where T : BlockData
        {
            var block = _contentRepository.GetDefault<T>(folder.ContentLink);
            ((IContent)block).Name = name;
            return block;
        }

        private T InitPage<T>(ContentReference parent, string title) where T : GenericContainerPage
        {
            var page = _contentRepository.GetDefault<T>(parent);
            page.PageName = title;
            page.Title = title;
            page.URLSegment = _urlSegmentCreator.Create(page);
            return page;
        }
        private ContentReference Save<T>(T page) where T : IContent
        {
            return _contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess);
        }

    }
}