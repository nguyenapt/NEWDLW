using Castle.Core.Internal;
using EPiServer.Core;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Navigation.ViewModels
{
    public class SidebarNavigationItemModel : NavigationItemModel
    {
        public SidebarNavigationItemModel(PageData page)
        {
            HasTemplate = page != null && page.HasTemplate();
        }
        public IEnumerable<SidebarNavigationItemModel> Children { get; set; }

        public bool HasTemplate { get; private set; }
    }
}