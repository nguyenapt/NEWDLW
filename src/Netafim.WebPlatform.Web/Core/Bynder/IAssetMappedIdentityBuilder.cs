using EPiServer.DataAbstraction;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public interface IAssetMappedIdentityBuilder
    {
        MappedIdentity GetMappedIdentity(string assetId);
    }
}