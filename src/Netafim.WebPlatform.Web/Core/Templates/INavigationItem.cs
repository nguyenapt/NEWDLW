using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    public interface INavigationItem : IContent
    {
        string NavigationTitle { get; }
        ContentReference NavigationImageLink { get; }

        ContentReference NavigationLink { get; }

        bool DisplayInSidebarNavigation { get; }

        bool DisplaySidebarNavigation { get; }
    }
}