using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Navigation.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public interface INavigationRepository
    {
        IEnumerable<MainNavigationItemModel> GetMainNavItems(ContentReference navigationRoot, ContentReference currentPageLink, bool excludeInvisible = true);
        IEnumerable<SidebarNavigationItemModel> GetSidebarItems(INavigationItem page);
    }
}