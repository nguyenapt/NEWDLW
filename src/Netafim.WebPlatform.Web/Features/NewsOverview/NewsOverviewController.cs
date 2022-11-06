using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.NewsOverview
{
    public class NewsOverviewController : ListingBaseBlockController<NewsOverviewBlock, NewsOverviewQueryViewModel, NewsPage>
    {
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly INewsRepository _newsRepository;

        private const int DefaultYearsToBackward = 5;

        public NewsOverviewController(IContentLoader contentLoader,
            IPageService pageService,
            IFindSettings findSettings,
            IPageRouteHelper pageRouteHelper,
            INewsRepository productFamilyRepo)
            : base(contentLoader, pageService, findSettings)
        {
            _pageRouteHelper = pageRouteHelper;
            _newsRepository = productFamilyRepo;
        }
        public override ActionResult Index(NewsOverviewBlock currentContent)
        {
            var yearsRange = GetYearsRange(currentContent);
            var model = new NewsOverviewViewModel(currentContent)
            {
                Years = yearsRange,
                LatestYearHavingNews = _newsRepository.GetLatestYearHavingNews(yearsRange)
            };
            return PartialView(FullViewPath(currentContent), model);
        }

        private IEnumerable<int> GetYearsRange(NewsOverviewBlock currentContent)
        {
            var yearsToBackward = currentContent.TotalYearsToDisplayNews > 0 ? currentContent.TotalYearsToDisplayNews : DefaultYearsToBackward;
            var range = Enumerable.Range(DateTime.Now.Year - yearsToBackward + 1, yearsToBackward);
            return range.Reverse();
        }
        protected override string ResultViewPath(NewsOverviewBlock model)
        {
            return string.Format(Core.Templates.Global.Constants.AbsoluteViewPathFormat, "NewsOverview", "_searchResult");
        }
    }
}