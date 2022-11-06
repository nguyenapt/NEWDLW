using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Navigation.ViewModels;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public class NavigationController : Controller
    {
        private readonly INavigationSettings _navSettings;
        private readonly INavigationRepository _navRepo;
        private readonly IPageRouteHelper _pageRouteHelper;

        public NavigationController(INavigationSettings navSettings, INavigationRepository navRepo, IPageRouteHelper pageRouterHelper)
        {
            _navSettings = navSettings;
            _navRepo = navRepo;
            _pageRouteHelper = pageRouterHelper;
        }

        public ActionResult Index()
        {
            var currentPageLink = this.ControllerContext.RequestContext.GetContentLink();
            var viewModel = _navRepo.GetMainNavItems(_navSettings.NavigationRoot, currentPageLink, true);
            return PartialView("_MainNavigation", viewModel);
        }

        public ActionResult SidebarNavigation()
        {
            var page = _pageRouteHelper.Page as INavigationItem;
            if (page == null || !page.DisplaySidebarNavigation) return new EmptyResult();
            var viewModel = _navRepo.GetSidebarItems(page);
            return PartialView("_SidebarNavigation", viewModel);
        }
    }
}