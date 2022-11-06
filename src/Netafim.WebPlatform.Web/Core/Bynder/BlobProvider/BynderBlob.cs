using System;
using System.IO;
using System.Linq;
using System.Net;
using EPiServer.Framework.Blobs;
using Newtonsoft.Json;

namespace Netafim.WebPlatform.Web.Core.Bynder.BlobProvider
{
    public class BynderBlob : Blob
    {
        private static readonly object @lock = new object();

        private readonly AssetData _assetData;
        private readonly IBynderBlobCache _blobCache;
        private string _derivative;

        public BynderBlob(Uri id, AssetData assetData, IBynderBlobCache blobBlobCache) : base(id)
        {
            _derivative = GetImageDerivative(id);
            _assetData = assetData;
            _blobCache = blobBlobCache;
        }

        private string GetImageDerivative(Uri id)
        {
            // TODO make more robust

            var filename = id.Segments.Last();

            var imageType = filename.Split(new []{ '-'}, StringSplitOptions.RemoveEmptyEntries)[0];

            return imageType;
        }

        public override Stream OpenRead()
        {
            switch (_derivative)
            {
                case ImageDerivative.Thumbnail:
                    return GetBinaryData(_assetData.ThumbnailUrl);
                default:
                    return GetBinaryData(_assetData.Derivatives.First().ImageUrl);
            }
        }

        private Stream GetBinaryData(string assetUrl)
        {
            // check again if cache is not already populated
            // TODO load correct derivative from cache
            var data = _blobCache.GetBlob(ID);

            if (data == null)
            {
                lock (@lock)
                {
                    data = _blobCache.GetBlob(ID);

                    if (data == null)  // make sure that waiting thread is not executing second time
                    {
                        var stream = GetBinaryDataFromSource(assetUrl);
                        var blob = _blobCache.StoreBlob(ID, stream);

                        using (var s = GenerateStreamFromString(JsonConvert.SerializeObject(_assetData, Formatting.Indented)))
                        {
                            _blobCache.StoreBlob(CreateAsseDataUri(), s);
                        }

                        return blob.OpenRead();
                    }
                }
            }

            return data.OpenRead();
        }

        private Stream GetBinaryDataFromSource(string assetUrl)
        {
            using (var webClient = new WebClient())
            {
                return webClient.OpenRead(assetUrl);
            }
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        public Uri CreateAsseDataUri()
        {
            return new Uri($"{GetContainerIdentifier(ID)}asset-data.json");
        }
    }
}