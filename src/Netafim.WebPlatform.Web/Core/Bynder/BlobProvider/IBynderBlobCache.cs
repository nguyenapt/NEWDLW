using System;
using System.IO;
using EPiServer.Framework.Blobs;

namespace Netafim.WebPlatform.Web.Core.Bynder.BlobProvider
{
    public interface IBynderBlobCache
    {
        // TODO rename method? changes default behavior (method can return null)
        Blob GetBlob(Uri id);

        Blob StoreBlob(Uri id, Stream data);
    }
}