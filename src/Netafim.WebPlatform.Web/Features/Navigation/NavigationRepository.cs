using Castle.Core.Internal;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Navigation.ModelFactories;
using Netafim.WebPlatform.Web.Features.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public class NavigationRepository : INavigationRepository
    {
        private readonly IContentRepository _contentRepo;
        private readonly IEnumerable<IDoormatModelFactory> _doormatModelFactories;
        private readonly IClient _searchClient;
        protected readonly IPageService _pageService;
        protected readonly IFindSettings _findSettings;

        public NavigationRepository(IContentRepository contentRepo,
            IEnumerable<IDoormatModelFactory> doormatModelFactories,
            IClient client,
            IPageService pageSearch,
            IFindSettings findSettings)
        {
            _contentRepo = contentRepo;
            _doormatModelFactories = doormatModelFactories;
            _searchClient = client;
            _pageService = pageSearch;
            _findSettings = findSettings;
        }

        public IEnumerable<MainNavigationItemModel> GetMainNavItems(ContentReference navigationRoot, ContentReference currentPageLink, bool excludeInvisible = true)
        {
            var emptyResult = new List<MainNavigationItemModel>();
            if (ContentReference.IsNullOrEmpty(navigationRoot)) return emptyResult;

            NavigationContainerPage navContainerPage;
            if (!_contentRepo.TryGet(navigationRoot, out navContainerPage))
            {
                return emptyResult;
            }
            var navItemSettings = _contentRepo.GetChildren<NavigationPage>(navigationRoot).FilterForDisplay(false, excludeInvisible);
            return navItemSettings.IsNullOrEmpty() ? emptyResult : navItemSettings.Where(item => item.Link != null).Select(x => MapToMainNavItemModel(x, currentPageLink, excludeInvisible)).ToList();
        }

        private MainNavigationItemModel MapToMainNavItemModel(NavigationPage item, ContentReference currentPageLink, bool excludeInvisible = true)
        {

            var doormatModelFactory = _doormatModelFactories.FirstOrDefault(x => x.IsSatisfied(item));
            if (doormatModelFactory == null) { throw new System.Exception("Cannot find the satisfied Doormat Factory."); }

            return new MainNavigationItemModel()
            {
                DoormatType = (DoormatType)item.DoormatType,
                Link = item.Link,
                Title = GetTitleWithFallback(item),
                IsActive = item.Link.IsActiveNavigationNode(currentPageLink),
                DoormatItems = doormatModelFactory.Create(item, currentPageLink, excludeInvisible),
                ViewAllText = item.ViewAllText
            };
        }

        private string GetTitleWithFallback(NavigationPage item)
        {
            if (!string.IsNullOrEmpty(item.Title)) return item.Title;

            GenericContainerPage linkedPage;
            if (!_contentRepo.TryGet(item.Link, out linkedPage)) return string.Empty;

            return linkedPage.Title ?? linkedPage.Name;
        }

        public IEnumerable<SidebarNavigationItemModel> GetSidebarItems(INavigationItem page)
        {
            var emptyResult = Enumerable.Empty<SidebarNavigationItemModel>();
            if (page == null || !page.DisplaySidebarNavigation) return emptyResult;
            var filter = CreateSidebarFilter(page);
            var navPages = _pageService.GetContents(_findSettings.MaxItemsPerRequest, filter.Expression);
            var navPagesLevel1 = navPages.Where(x => x.ContentLink.Equals(page.ContentLink) || x.ParentLink.Equals(page.ParentLink));
            return navPagesLevel1.IsNullOrEmpty() ? emptyResult : navPagesLevel1.Select(x => CreateSidebarItem(page, x, navPages));
        }

        SidebarNavigationItemModel CreateSidebarItem(INavigationItem curPage, INavigationItem itemPage, IEnumerable<INavigationItem> allItemPages)
        {
            var model = new SidebarNavigationItemModel((PageData)itemPage)
            {
                Link = itemPage.ContentLink,
                Title = itemPage.NavigationTitle
            };
            var isActive = itemPage.ContentLink.Equals(curPage.ContentLink) || itemPage.ContentLink.Equals(curPage.ParentLink);
            model.IsActive = isActive;
            model.Children = isActive
                ? allItemPages.Where(x => x.ParentLink.Equals(itemPage.ContentLink)).Select(x => CreateSidebarItem(curPage, x, allItemPages))
                : Enumerable.Empty<SidebarNavigationItemModel>();
            return model;
        }
        private FilterExpression<INavigationItem> CreateSidebarFilter(INavigationItem currentPage)
        {
            var filterBuilder = _searchClient.BuildFilter<INavigationItem>();
            filterBuilder = filterBuilder.Or(x => x.ParentLink.ID.Match(currentPage.ContentLink.ID));
            filterBuilder = filterBuilder.Or(x => x.ParentLink.ID.Match(currentPage.ParentLink.ID));
            filterBuilder = filterBuilder.And(x => x.DisplayInSidebarNavigation.Match(true));

            return new FilterExpression<INavigationItem>(m => filterBuilder);
        }
    }
}