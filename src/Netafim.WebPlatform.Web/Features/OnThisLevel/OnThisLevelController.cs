using System;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.OnThisLevel
{
    public class OnThisLevelController : BlockController<OnThisLevelBlock>
    {
        protected readonly IPageService SearchService;
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly IOnThisLevelSettings _onThisLevelSettings;

        public OnThisLevelController(
            IPageService searchService, 
            IPageRouteHelper pageRouteHelper,
            IOnThisLevelSettings onThisLevelSettings
            )
        {
            SearchService = searchService;
            _pageRouteHelper = pageRouteHelper;
            _onThisLevelSettings = onThisLevelSettings;
        }

        public override ActionResult Index(OnThisLevelBlock currentBlock)
        {
            if (currentBlock == null) throw new ArgumentNullException(nameof(currentBlock));

            var model = new OnThisLevelViewModel(currentBlock)
            {
                SiblingPages = SearchService.GetSiblingPages<PageBase>(_onThisLevelSettings.MaxLinksInOnThisLevelBlock, _pageRouteHelper.PageLink)
            };
            return PartialView("_onThisLevelBlock", model);
        }
    }
}
