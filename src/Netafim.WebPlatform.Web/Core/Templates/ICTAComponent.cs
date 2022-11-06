using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    public interface ICTAComponent
    {
        string LinkText { get; }
        ContentReference Link { get; }
    }
}
