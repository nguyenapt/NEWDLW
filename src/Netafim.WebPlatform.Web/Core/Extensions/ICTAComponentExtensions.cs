using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class ICTAComponentExtensions
    {
        public static bool CanDisplayCTA(this ICTAComponent component)
        {
            return component != null && !string.IsNullOrWhiteSpace(component.LinkText) && !ContentReference.IsNullOrEmpty(component.Link);
        }
    }
}