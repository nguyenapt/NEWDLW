using System;
using System.IO;
using EPiServer.Azure.Blobs;
using EPiServer.Framework.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Netafim.WebPlatform.Web.Core.Bynder.BlobProvider
{
    public class AzureCacheBlobProvider : AzureBlobProvider, IBynderBlobCache
    {
        private CloudBlobContainer _container;

        public override void Initialize(string connectionString, string containerName)
        {
            base.Initialize(connectionString, containerName);

            _container = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient().GetContainerReference(containerName);
        }

        public override Blob GetBlob(Uri id)
        {
            if (!_container.GetBlockBlobReference(GetAzureRelativeAddress(id)).Exists())
            {
                return null;
            }

            return base.GetBlob(id);
        }

        public Blob StoreBlob(Uri id, Stream data)
        {
            var cacheBlob = base.GetBlob(id);

            cacheBlob.Write(data);

            return cacheBlob;
        }

        private string GetAzureRelativeAddress(Uri id)
        {
            return id.AbsolutePath.Substring(1);
        }
    }
}