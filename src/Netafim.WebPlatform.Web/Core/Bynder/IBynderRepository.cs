using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public interface IBynderRepository
    {
        AssetData GetAsset(string id);

        IEnumerable<AssetInfo> GetAssets();

        IEnumerable<AssetInfo> FindAssets(string query);
    }
}