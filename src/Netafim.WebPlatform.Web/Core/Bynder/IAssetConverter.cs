using System;
using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public interface IAssetConverter
    {
        MediaData ConvertToContent(AssetData data);

        Type ResolveContentType(AssetData data);

        Type ResolveContentType(AssetInfo info);
    }
}