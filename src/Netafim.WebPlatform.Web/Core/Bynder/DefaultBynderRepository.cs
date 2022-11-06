using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Bynder.Api;
using Bynder.Api.Queries;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public class DefaultBynderRepository : IBynderRepository
    {
        private readonly IBynderSettings _settings;

        private IBynderApi _bynderApi;

        public DefaultBynderRepository(IBynderSettings settings)
        {
            _settings = settings;

            _bynderApi = BynderApiFactory.Create(new Settings
            {
                CONSUMER_KEY = settings.ConsumerKey,
                CONSUMER_SECRET = settings.ConsumerSecret,
                TOKEN = settings.Token,
                TOKEN_SECRET = settings.TokenSecret,
                URL = settings.ApiBaseUrl
            });
        }

        public AssetData GetAsset(string id)
        {
            var assetBankManager = _bynderApi.GetAssetBankManager();

            var media = assetBankManager.RequestMediaInfoAsync(new MediaInformationQuery()
            {
                MediaId = id
            }).Result;

            // TODO fetch regular and square version
            var defaultWebVersion = media.MediaItems.FirstOrDefault(x => x.Type != "original");
            string defaultWebVersionUrl = null;

            if (defaultWebVersion != null)
            {
                var defaultWebVersionAsset = assetBankManager.GetDownloadFileUrlAsync(new DownloadMediaQuery()
                {
                    MediaId = id,
                    MediaItemId = defaultWebVersion.Id
                }).Result;

                defaultWebVersionUrl = defaultWebVersionAsset.ToString();

                // TO BE
                // defaultWebVersionUrl = defaultWebVersion.Thumbnails.WebImage;
            }

            var assetData = new AssetData()
            {
                Id = media.Id,
                Name = media.Name,
                Extension = media.Extension.FirstOrDefault(),
                ThumbnailUrl = media.Thumbnails.Mini,
                Created = DateTime.Parse(media.DateCreated, CultureInfo.InvariantCulture),
                LastModified = DateTime.Parse(media.DateModified, CultureInfo.InvariantCulture)
            };

            assetData.Derivatives = new List<DerivativeData>()
            {
                new DerivativeData()
                {
                    Key = ImageDerivative.Large,
                    ImageUrl = defaultWebVersionUrl ?? media.Thumbnails.WebImage
                }
            };

            return assetData;
        }

        public IEnumerable<AssetInfo> GetAssets()
        {
            var assetBankManager = _bynderApi.GetAssetBankManager();

            var mediaList = assetBankManager.RequestMediaListAsync(new MediaQuery
            {
                Page = 1,
                Limit = _settings.MaxAssetsToFetch
            }).Result;

            foreach (var media in mediaList)
            {
                yield return new AssetInfo()
                {
                    Id = media.Id,
                    Name = media.Name,
                    Extension = media.Extension.First() // TODO handle multiple extension
                };
            }
        }

        public IEnumerable<AssetInfo> FindAssets(string query)
        {
            // TODO block empty query? or use default bynder?

            var assetBankManager = _bynderApi.GetAssetBankManager();

            var mediaList = assetBankManager.RequestMediaListAsync(new MediaQuery
            {
                Keyword = query,
                Page = 1,
                Limit = _settings.MaxAssetsToFetch
            }).Result;

            foreach (var media in mediaList)
            {
                yield return new AssetInfo()
                {
                    Id = media.Id,
                    Name = media.Name,
                    Extension = media.Extension.First() // TODO handle multiple extension
                };
            }
        }

        private byte[] GetBinaryData(string assetUrl)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadData(assetUrl);
            }
        }

    }
}