using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using EPiServer.Framework.Blobs;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Core.Bynder.BlobProvider
{
    public class BynderBlobProvider : EPiServer.Framework.Blobs.BlobProvider
    {
        public const string Key = "bynder";

        private IBynderRepository _bynderRepository;
        private IBlobProviderRegistry _blobProviderRegistry;
        private IBynderBlobCache _bynderBlobCache;

        public BynderBlobProvider()
        {
            _bynderRepository = ServiceLocator.Current.GetInstance<IBynderRepository>();
            _blobProviderRegistry = ServiceLocator.Current.GetInstance<IBlobProviderRegistry>();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            string cacheBlobProviderName = config["cacheBlobProvider"];
            if (string.IsNullOrWhiteSpace(cacheBlobProviderName))
                throw new ConfigurationErrorsException("CacheBlobProvider must be set.");

            Initialize(cacheBlobProviderName);
        }

        private void Initialize(string cacheBlobProviderName)
        {
            var cacheProvider = _blobProviderRegistry.GetProvider(GenerateUri(cacheBlobProviderName));

            _bynderBlobCache = cacheProvider as IBynderBlobCache;

            if (_bynderBlobCache == null) throw new Exception($"{cacheBlobProviderName} does not inherit {nameof(IBynderBlobCache)}");
        }

        public override Blob CreateBlob(Uri id, string extension)
        {
            throw new NotImplementedException();
        }

        public override Blob GetBlob(Uri id)
        {
            // check here already bynder blob cache to avoid call to bynder api
            if (TryGetCachedBlob(id, out var cachedBlob))
            {
                return cachedBlob;
            }

            var container = id.Segments.Where(s => s != "/").First();
            var assetId = container.Replace("_", "-").Remove(container.Length -1); // remove trailing slash

            var assetData = _bynderRepository.GetAsset(assetId); // TODO move to blob for performance reason?

            return new BynderBlob(id, assetData, _bynderBlobCache);
        }

        private bool TryGetCachedBlob(Uri id, out Blob cachedBlob)
        {
            cachedBlob = _bynderBlobCache.GetBlob(id);

            if (cachedBlob != null)
            {
                return true;
            }

            return false;
        }

        public override void Delete(Uri id)
        {
            throw new NotImplementedException();
        }

        public static Uri GenerateBlobUri(string assetId, string type, string filename)
        {
            // TODO use Blob.GetContainerIdentifier(string provider, Guid container)
                // use content id as container
                // same bynder asset can be used in multiple icontent instances

            // epi uri validation fails on dashes
            // TODO use Blob.GetContainerIdentifier()
            var container = assetId.Replace("-", "_");

            // TO BE
            //var container = Blob.GetContainerIdentifier(Key, Guid.Parse(assetId));
            //var uri = new Uri(string.Format("{0}{1}_{2}{3}", container, Path.GetFileNameWithoutExtension(filename), type, $".{Path.GetExtension(filename)}"));


            return new Uri($"{Blob.BlobUriScheme}://{Key}/{container}/{type}-{filename}");
        }

        public static Uri GenerateUri(string providerKey, string container = "container", string filename = "filename")
        {
            return new Uri($"{Blob.BlobUriScheme}://{providerKey}/{container}/{filename}");
        }
    }
}