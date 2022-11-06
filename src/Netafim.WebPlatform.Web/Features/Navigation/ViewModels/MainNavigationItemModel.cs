using Castle.Core.Internal;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Navigation.ViewModels
{
    public class MainNavigationItemModel : NavigationItemModel
    {
        public DoormatType DoormatType { get; set; }

        public IEnumerable<DoormatNavigationItemModel> DoormatItems{get;set;}

        public string ViewAllText { get; set; }

        public bool HasDoormat { get { return !DoormatItems.IsNullOrEmpty(); } }

    }
}