using Castle.Core.Internal;
using EPiServer.Core;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.Navigation.ViewModels
{
    public class DoormatNavigationItemModel : NavigationItemModel
    {
        public DoormatNavigationItemModel( PageData page)
        {
            this.HasTemplate = page != null && page.HasTemplate();
        }
        public ContentReference ImageLink { get; set; }
        public IEnumerable<DoormatNavigationItemModel> Children { get; set; }

        public bool IsLeaf
        {
            get
            {
                return Children.IsNullOrEmpty() || Children.All(x => x == null);
            }
        }

        public IEnumerable<DoormatNavigationItemModel> Leaves
        {
            get
            {
                return IsLeaf ? new List<DoormatNavigationItemModel>() { this } : Children;
            }
        }

        public bool IsImageNode
        {
            get
            {
                return !ContentReference.IsNullOrEmpty(ImageLink);
            }
        }        

        public bool HasTemplate { get; private set; }
    }
}