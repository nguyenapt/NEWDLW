using Castle.Core.Internal;
using EPiServer.Framework.Cache;
using Netafim.WebPlatform.Web.Core.Caching;
using Netafim.WebPlatform.Web.Features.Layout;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class FacebookService : SocialService
    {
        private readonly string _fbApiGetTokenUrl = "https://graph.facebook.com/oauth/access_token";
        protected string _fbFeedUrl => $"https://graph.facebook.com/v2.11/{_socialName}/feed";
        private readonly string _fbHome = "https://facebook.com/";
        private string _fbTokenCacheKey => $"fb_{_socialName}_accessToken";
        private string _fbFeedCacheKey => $"fb_{_socialName}_{_totalItems}_feeds";
        private readonly CacheOptions _fbTokenCacheOption;
        private readonly CacheOptions _fbFeedCacheOption;
        private readonly IFacebookAppSettings _facebookAppSettings;
        

        public FacebookService(IFacebookAppSettings facebookAppSettings, ISocialMediaSettings socialMediaSettings, ICacheProvider cacheProvider) : base(cacheProvider, socialMediaSettings)
        {
            _facebookAppSettings = facebookAppSettings;
            _fbTokenCacheOption = new CacheOptions(CacheTimeoutType.Absolute, TimeSpan.FromDays(59));
            _fbFeedCacheOption = new CacheOptions(CacheTimeoutType.Absolute, TimeSpan.FromHours(3));
        }

        protected override string GetAccessTokenCacheKey() => _fbTokenCacheKey;
        protected override CacheOptions GetAccessTokenCacheOptions() => _fbTokenCacheOption;

        protected override string GetFeedsCacheKey() => _fbFeedCacheKey;

        protected override CacheOptions GetFeedsCacheOptions() => _fbFeedCacheOption;

        protected override string GetSocialChannelLink() => _socialMediaSettings.FacebookLink;
        protected override string GetDefaultChannelName() => _facebookAppSettings.FacebookName;

        protected override string GetNewAccessToken()
        {
            var appId = _facebookAppSettings.FacebookAppId;
            var appSecret = _facebookAppSettings.FacebookAppSecret;
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(appSecret)) return string.Empty;

            var tokenRequestParams = new Dictionary<string, object>()
            {
                { "client_id", appId},
                { "client_secret", appSecret},
                { "grant_type", "client_credentials"}
            };
            IRestResponse response = HttpUtils.MakeRequest(_fbApiGetTokenUrl, tokenRequestParams);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return string.Empty;

            var tokenModel = JsonConvert.DeserializeObject<FacebookTokenModel>(response.Content);
            return tokenModel != null ? tokenModel.AccessToken : string.Empty;
        }

        protected override IRestResponse MakeRequestWithToken(int totalFeeds, string accessToken)
        {
            var feedsRequestParams = new Dictionary<string, object>()
            {
                {"limit",totalFeeds },
                {"access_token"  ,accessToken }
            };
            return HttpUtils.MakeRequest(_fbFeedUrl, feedsRequestParams);
        }
        protected override IEnumerable<FeedViewModel> ExtractFeedViewModels(IRestResponse apiResponse)
        {
            var fbFeeds = JsonConvert.DeserializeObject<FacebookFeed>(apiResponse.Content);
            if (fbFeeds == null || fbFeeds.Data.IsNullOrEmpty()) return Enumerable.Empty<FeedViewModel>();

            return fbFeeds.Data.Select(x => MapToFeedViewModel(x));
        }

        private FeedViewModel MapToFeedViewModel(FacebookPostItem fbFeed)
        {
            return new FeedViewModel()
            {
                Channel = SocialChannel.Facebook,
                ChannelLink = _socialMediaSettings.FacebookLink,
                Description = fbFeed.Message,
                PostDate = fbFeed.CreatedTime,
                PostLink = $"{_fbHome}{fbFeed.Id}"
            };
        }

        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting != null && channelSetting.Channel == SocialChannel.Facebook;
        }
    }
}
