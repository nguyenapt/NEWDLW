using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace Netafim.WebPlatform.Web.Core.Bynder.BlobProvider
{
    [EPiServerDataStore(AutomaticallyCreateStore = true)]
    public class BynderBlobCacheInfo : IDynamicData
    {
        public Identity Id { get; set; }

        public Uri Uri { get; set; }

        public bool Cached { get; set; }
    }
}