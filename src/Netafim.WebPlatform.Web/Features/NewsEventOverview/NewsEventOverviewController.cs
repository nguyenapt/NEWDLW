using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.NewsOverview;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.NewsEventOverview
{
    public class NewsEventOverviewController : BlockController<NewsListingBlock>
    {
        private readonly INewsRepository _newsRepo;
        public NewsEventOverviewController(INewsRepository newsRepo)
        {
            _newsRepo = newsRepo;
        }
        public override ActionResult Index(NewsListingBlock currentContent)
        {
            var viewModel = new NewsListingViewModel(currentContent)
            {
                AllNews = _newsRepo.GetLatestNewsPages(currentContent.MaximumNewsToDisplay)
            };
            return PartialView(currentContent.GetDefaultFullViewName(), viewModel);
        }
    }
}