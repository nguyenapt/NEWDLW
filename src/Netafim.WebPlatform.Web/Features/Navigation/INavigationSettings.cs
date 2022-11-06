using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public interface INavigationSettings
    {
        ContentReference NavigationRoot { get; }
    }
}