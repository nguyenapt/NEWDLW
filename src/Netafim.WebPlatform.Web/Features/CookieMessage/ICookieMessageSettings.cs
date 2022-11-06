using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.CookieMessage
{
    public interface ICookieMessageSettings
    {
        ContentReference CookieLink { get; }
    }
}