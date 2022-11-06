using System;
using System.IO;
using System.Linq;
using EPiServer.Data.Dynamic;
using EPiServer.Framework.Blobs;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Core.Bynder.BlobProvider
{
    public class FileCacheBlobProvider : FileBlobProvider, IBynderBlobCache
    {
        private readonly DynamicDataStoreFactory _dynamicDataStoreFactory;

        public FileCacheBlobProvider()
        {
            _dynamicDataStoreFactory = ServiceLocator.Current.GetInstance<DynamicDataStoreFactory>();
        }

        public override Blob GetBlob(Uri id)
        {
            var cacheInfo = GetCacheInfo(id);

            if (cacheInfo == null)
            {
                return null;
            }

            return base.GetBlob(id);
        }

        private BynderBlobCacheInfo GetCacheInfo(Uri id)
        {
            var store = _dynamicDataStoreFactory.CreateStore(typeof(BynderBlobCacheInfo));

            return store.Items<BynderBlobCacheInfo>().SingleOrDefault(x => x.Uri == id);
        }

        private void StoreCacheInfo(BynderBlobCacheInfo cacheInfo)
        {
            var store = _dynamicDataStoreFactory.CreateStore(typeof(BynderBlobCacheInfo));

            store.Save(cacheInfo);
        }

        public Blob StoreBlob(Uri id, Stream data)
        {
            var cacheBlob = base.GetBlob(id);

            cacheBlob.Write(data);

            StoreCacheInfo(new BynderBlobCacheInfo()
            {
                Uri = id,
                Cached = true
            });

            return cacheBlob;
        }
    }
}