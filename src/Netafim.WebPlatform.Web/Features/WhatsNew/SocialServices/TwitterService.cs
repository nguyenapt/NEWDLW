using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using EPiServer.Framework.Cache;
using EPiServer.Logging;
using Netafim.WebPlatform.Web.Core.Caching;
using Netafim.WebPlatform.Web.Features.Layout;
using Newtonsoft.Json;
using RestSharp;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class TwitterService : SocialService
    {
        private readonly string _twitterAccessTokenApiUrl = "https://api.twitter.com/oauth2/token";
        private readonly string _twitterFeedApiUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        private readonly string _twitterLinkFormat = "https://twitter.com/{0}/status/{1}";
        private string _twitterTokenCacheKey => $"twitter_{_socialName}_accessToken";
        private string _twitterFeedCacheKey => $"twitter_{_socialName}_{_totalItems}_feeds";
        private readonly CacheOptions _twitterTokenCacheOption;
        private readonly CacheOptions _twitterFeedCacheOption;
        private readonly ITwitterAppSettings _twitterAppSettings;

        private ILogger _logger = LogManager.GetLogger();

        public TwitterService(ITwitterAppSettings twitterAppSettings, ISocialMediaSettings socialMediaSettings, ICacheProvider cacheManager) : base(cacheManager, socialMediaSettings)
        {
            _twitterTokenCacheOption = new CacheOptions(CacheTimeoutType.Absolute, TimeSpan.FromDays(59));
            _twitterFeedCacheOption = new CacheOptions(CacheTimeoutType.Absolute, TimeSpan.FromHours(3));
            _twitterAppSettings = twitterAppSettings;
        }

        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting != null && channelSetting.Channel == SocialChannel.Twitter;
        }

        protected override IEnumerable<FeedViewModel> ExtractFeedViewModels(IRestResponse apiResponse)
        {
            var feeds = JsonConvert.DeserializeObject<object>(apiResponse.Content);
            try
            {
                var twFeeds = JsonConvert.DeserializeObject<IEnumerable<TwitterFeedItemModel>>(apiResponse.Content);

                if (twFeeds.IsNullOrEmpty()) { return null; }

                return twFeeds.Select(x => new FeedViewModel()
                {
                    Channel = SocialChannel.Twitter,
                    ChannelLink = _socialMediaSettings.TwitterLink,
                    Description = x.Text,
                    PostDate = x.CreateDateTime,
                    PostLink = string.Format(_twitterLinkFormat, _socialName, x.Id)
                });
            }
            catch (Exception)
            {
                _logger.Error("Cannot parse the Twitter response to TwitterFeedItemModel.");
                return null;
            }
        }

        protected override string GetAccessTokenCacheKey() => _twitterTokenCacheKey;

        protected override CacheOptions GetAccessTokenCacheOptions() => _twitterTokenCacheOption;

        protected override string GetDefaultChannelName() => _twitterAppSettings.TwitterName;

        protected override string GetFeedsCacheKey() => _twitterFeedCacheKey;

        protected override CacheOptions GetFeedsCacheOptions() => _twitterFeedCacheOption;

        protected override string GetNewAccessToken()
        {
            var consumerKey = _twitterAppSettings.TwitterConsumerKey;
            var consumerSecret = _twitterAppSettings.TwitterConsumerSecret;
            if (string.IsNullOrWhiteSpace(consumerKey) || string.IsNullOrWhiteSpace(consumerSecret)) return string.Empty;

            var tokenRequestParams = new Dictionary<string, object>()
            {
                { "grant_type", "client_credentials"}
            };
            var headerParams = new Dictionary<string, string>() {
                { "Authorization", $"Basic {Convert.ToBase64String( new UTF8Encoding().GetBytes($"{consumerKey}:{consumerSecret}"))}"}
            };
            IRestResponse response = HttpUtils.MakeRequest(_twitterAccessTokenApiUrl, tokenRequestParams, Method.POST, headerParams);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return string.Empty;

            var tokenModel = JsonConvert.DeserializeObject<TwitterTokenModel>(response.Content);
            return tokenModel != null ? tokenModel.AccessToken : string.Empty;
        }

        protected override string GetSocialChannelLink() => _socialMediaSettings.TwitterLink;

        protected override IRestResponse MakeRequestWithToken(int totalFeeds, string accessToken)
        {
            var feedsRequestParams = new Dictionary<string, object>()
            {
                {"screen_name",_socialName },
                {"count"  ,totalFeeds },
                {  "trim_user",true }
            };
            var headers = new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {accessToken}"}
            };
            return HttpUtils.MakeRequest(_twitterFeedApiUrl, feedsRequestParams, Method.GET, headers);
        }
    }
}
