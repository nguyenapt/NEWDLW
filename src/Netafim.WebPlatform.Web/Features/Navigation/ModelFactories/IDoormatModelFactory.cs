using EPiServer.Core;
using Netafim.WebPlatform.Web.Features.Navigation.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Navigation.ModelFactories
{
    public interface IDoormatModelFactory
    {
        bool IsSatisfied(NavigationPage navPage);

        IEnumerable<DoormatNavigationItemModel> Create(NavigationPage navPage, ContentReference currentLink, bool excludeInvisible);
    }
}