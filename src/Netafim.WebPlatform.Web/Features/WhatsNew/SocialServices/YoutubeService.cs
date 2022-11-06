using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using EPiServer.Framework.Cache;
using Netafim.WebPlatform.Web.Core.Caching;
using Netafim.WebPlatform.Web.Features.Layout;
using Newtonsoft.Json;
using RestSharp;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class YoutubeService : SocialService
    {
        private const string FeedUrl = "https://www.googleapis.com/youtube/v3/activities";

        private const string YouTubeParamPart = "snippet,contentDetails";
        private string YoutubeFeedCacheKey => $"youtube{_socialName}_{_totalItems}_feeds";
        private readonly CacheOptions _youtubeTokenCacheOption;
        private readonly CacheOptions _youtubeFeedCacheOption;
        private readonly IYouTubeAppSettings _youTubeAppSettings;

        public YoutubeService(IYouTubeAppSettings youTubeAppSettings, ICacheProvider cacheManager, ISocialMediaSettings socialMediaSettings) : base(cacheManager, socialMediaSettings)
        {
            _youTubeAppSettings = youTubeAppSettings;
            _youtubeTokenCacheOption = new CacheOptions(CacheTimeoutType.Absolute, TimeSpan.FromDays(59));
            _youtubeFeedCacheOption = new CacheOptions(CacheTimeoutType.Absolute, TimeSpan.FromHours(3));
        }

        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting != null && channelSetting.Channel == SocialChannel.Youtube;
        }

        protected override IEnumerable<FeedViewModel> ExtractFeedViewModels(IRestResponse apiResponse)
        {
            var youtubeFeeds = JsonConvert.DeserializeObject<YoutubeFeedModels>(apiResponse.Content);
            if (youtubeFeeds == null || youtubeFeeds.Items.IsNullOrEmpty())
                return Enumerable.Empty<FeedViewModel>();

            return youtubeFeeds.Items.Select(MapToFeedViewModel);
        }

        protected override string GetAccessTokenCacheKey() => _youTubeAppSettings.YouTubeApiKey;

        protected override CacheOptions GetAccessTokenCacheOptions() => _youtubeTokenCacheOption;

        protected override string GetDefaultChannelName() => _socialMediaSettings.YoutubeChannel;

        protected override string GetFeedsCacheKey() => YoutubeFeedCacheKey;

        protected override CacheOptions GetFeedsCacheOptions() => _youtubeFeedCacheOption;

        protected override string GetNewAccessToken() => _youTubeAppSettings.YouTubeApiKey;

        protected override string GetSocialChannelLink() => _socialMediaSettings.YoutubeLink;

        protected override IRestResponse MakeRequestWithToken(int totalFeeds, string accessToken)
        {
            var requestParam = new Dictionary<string, object>
            {
                {"maxResults", totalFeeds },
                {"channelId", _socialMediaSettings.YoutubeChannel },
                {"part", YouTubeParamPart },
                {"key", accessToken }
            };

            return HttpUtils.MakeRequest(FeedUrl, requestParam);
        }

        private FeedViewModel MapToFeedViewModel(YoutubeFeedModels.YoutubeFeedItem youtubeFeed)
        {
            return new FeedViewModel
            {
                Channel = SocialChannel.Youtube,
                ChannelLink = _socialMediaSettings.YoutubeLink,
                Description = youtubeFeed.Snippet.Description,
                PostDate = youtubeFeed.Snippet.PublishedAt,
                Thumbnail = youtubeFeed.Snippet.ThumbnailMedium.Medium.Url,
                PostLink = $"https://www.youtube.com/watch?v={youtubeFeed.ContentDetails.Upload.VideoId}"
            };
        }
    }
}
