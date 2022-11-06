using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.Navigation.ViewModels
{
    public class NavigationItemModel
    {
        public string Title { get; set; }
        public ContentReference Link { get; set; }
        public bool IsActive { get; set; }
    }
}