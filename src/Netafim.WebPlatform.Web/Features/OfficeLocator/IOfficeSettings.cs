using EPiServer.Core;
namespace Netafim.WebPlatform.Web.Features.OfficeLocator
{
    public interface IOfficeSettings
    {
        ContentReference OfficeContainer { get; }
        int Radius { get; }
    }
}
