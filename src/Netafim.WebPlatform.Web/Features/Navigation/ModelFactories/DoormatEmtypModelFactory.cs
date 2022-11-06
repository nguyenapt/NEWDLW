using EPiServer.Core;
using Netafim.WebPlatform.Web.Features.Navigation.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Navigation.ModelFactories
{
    public class DoormatEmtypModelFactory : IDoormatModelFactory
    {
        public IEnumerable<DoormatNavigationItemModel> Create(NavigationPage navPage, ContentReference currentLink, bool excludeInvisible)
        {
            return null;
        }

        public bool IsSatisfied(NavigationPage navPage)
        {
            return navPage != null && (DoormatType)navPage.DoormatType == DoormatType.None;
        }
    }
}