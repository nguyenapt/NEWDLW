using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Dlw.EpiBase.Content.Cms.Search;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Find;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Repository;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.CropsOverview;
using Netafim.WebPlatform.Web.Features.SuccessStory;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview
{
    [ContentGenerator(Order = 120)]
    public class SuccessStoryOverviewContentGenerator : IContentGenerator
    {
        private const string Thumbnail1 = "~/Features/SuccessStoryOverview/Data/Demo/620x370.png";

        private readonly IContentRepository _contentRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IPageService _pageService;
        private readonly IFindSettings _findSettings;

        public SuccessStoryOverviewContentGenerator(IContentRepository contentRepository, CategoryRepository categoryRepository,
            ICountryRepository countryRepository, IPageService pageService, IFindSettings findSettings)
        {
            _contentRepository = contentRepository;
            _categoryRepository = categoryRepository;
            _countryRepository = countryRepository;
            _pageService = pageService;
            _findSettings = findSettings;
        }

        public void Generate(ContentContext context)
        {
            CreateSuccessStoryOverviewPage(context);
        }

        private void CreateSuccessStoryOverviewPage(ContentContext context)
        {
            var successStoryOverview = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            successStoryOverview.PageName = "Success Stories Overview";

            Save(successStoryOverview);

            successStoryOverview.Content = successStoryOverview.Content ?? new ContentArea();

            CreateSuccessStoryOverviewFilterBlock(successStoryOverview);

            CreateSuccessStoryPages(successStoryOverview);

            Save(successStoryOverview);
        }

        private void CreateSuccessStoryOverviewFilterBlock(GenericContainerPage successStoryOverview)
        {
            var successStoryFilter = this._contentRepository.GetDefault<SuccessStoryFilterBlock>(successStoryOverview.ContentLink).CreateWritableClone() as SuccessStoryFilterBlock;
            ((IContent)successStoryFilter).Name = "Success stories filter";
            successStoryFilter.Watermark = "Success Stories";
            successStoryFilter.Title = "What success stories are you looking for?";
            successStoryFilter.PageSize = 3;
            successStoryFilter.BackgroundColor = BackgroundColor.Grey;
            successStoryFilter.VerticalText = "Success Stories";

            successStoryOverview.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)successStoryFilter) });
        }

        private void CreateSuccessStoryPages(GenericContainerPage successStoryOverview)
        {
            var cropPages = _pageService.GetPages<CropsPage>(_findSettings.MaxItemsPerRequest, m => m.MatchType(typeof(CropsPage)));
            var cropDic = cropPages.IsNullOrEmpty() ?
                            new Dictionary<int, string>() :
                            cropPages.ToDictionary(t => t.ContentLink.ID, t => string.IsNullOrEmpty(t.Title) ? t.PageName : t.Title);

            for (int i = 0; i < 10; i++)
            {
                var country = GetRandomFromDictionary(_countryRepository.GetCountries());
                var crop = GetRandomFromDictionary(cropDic);
                GenerateSuccessStoryPage(successStoryOverview, country, crop, i);
            }
        }

        private void GenerateSuccessStoryPage(GenericContainerPage successStoryOverview, string country, int crop, int sequentialNumber)
        {
            var bigProject = _categoryRepository.Get(typeof(Big).Name);

            var storyPage = this._contentRepository.GetDefault<SuccessStoryPage>(successStoryOverview.ContentLink).CreateWritableClone() as SuccessStoryPage;
            storyPage.PageName = $"Success Story {sequentialNumber}";
            storyPage.Title = $"Success Story {sequentialNumber}";
            storyPage.Text = new XhtmlString("<p>Each hectare that was drip irrigated covered it's costs quicly.</p>");
            storyPage.Quote = new XhtmlString("<p>Dino Dalmasso from Morro Alto Farm</p><p>Corn, Brazil, 2016</p>");
            storyPage.Image = storyPage.CreateBlob(Thumbnail1);

            storyPage.Country = country;
            storyPage.CropId = crop;
            if (sequentialNumber%2 == 0)
            {
                storyPage.BoostedFrom = DateTime.Now.AddDays(sequentialNumber + 1);
                storyPage.BoostedTo = DateTime.Now.AddDays(sequentialNumber + 3);
            }
            else
            {
                storyPage.ProjectDate = DateTime.Now.AddDays(-sequentialNumber);
            }
                

            var pageRef = Save(storyPage);

            var newPage = _contentRepository.Get<SuccessStoryPage>(pageRef).CreateWritableClone();
            if (sequentialNumber % 2 == 0)
                newPage.Category.Add(bigProject.ID);

            Save(newPage);
        }

        private ContentReference Save(IContent content)
        {
            return _contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private static TKey GetRandomFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> source)
        {
            if (!source.Any())
                return (TKey)Convert.ChangeType(null, typeof(TKey));

            var randNum = new Random();
            var index = randNum.Next(0, source.Keys.Count - 1);
            var randomKey = source.Keys.ToArray()[index];
            if (source.ContainsKey(randomKey))
                return (TKey)Convert.ChangeType(source[randomKey], typeof(TKey));

            return (TKey)Convert.ChangeType(null, typeof(TKey)); 
        }
    }
}