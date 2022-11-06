using System;
using System.Collections.Generic;
using EPiServer.Framework.Cache;
using Netafim.WebPlatform.Web.Core.Caching;
using Netafim.WebPlatform.Web.Features.Layout;
using Newtonsoft.Json;
using RestSharp;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class LinkedInService : SocialService
    {
        private readonly ILinkedInAppSettings _linkedInAppSettings;

        public LinkedInService(ISocialMediaSettings socialMediaSettings,
            ILinkedInAppSettings linkedinAppSettings,
            ICacheProvider cacheManager)
            : base(cacheManager, socialMediaSettings)
        {
            this._linkedInAppSettings = linkedinAppSettings;
        }

        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting != null && channelSetting.Channel == SocialChannel.LinkedIn;
        }

        protected override IEnumerable<FeedViewModel> ExtractFeedViewModels(IRestResponse apiResponse)
        {
            return null;
        }

        protected override string GetAccessTokenCacheKey() => $"linkedin_{this._linkedInAppSettings.LinkedInAppId}_accesstoken_key";

        protected override CacheOptions GetAccessTokenCacheOptions() => new CacheOptions(CacheTimeoutType.Absolute, TimeSpan.FromSeconds(this._linkedInAppSettings.LinkedInTokenExpired));

        protected override string GetDefaultChannelName() => this._linkedInAppSettings.LinkedInAppName;

        protected override string GetFeedsCacheKey() => $"linkedin_{this._linkedInAppSettings.LinkedInAppId}_{_totalItems}_feeds";

        protected override CacheOptions GetFeedsCacheOptions() => new CacheOptions(CacheTimeoutType.Absolute, TimeSpan.FromSeconds(this._linkedInAppSettings.LinkedInFeedExpired));

        protected override string GetNewAccessToken()
        {
            var appId = _linkedInAppSettings.LinkedInAppId;
            var appSecret = _linkedInAppSettings.LinkedInAppSecret;
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(appSecret)) return string.Empty;

            var tokenRequestParams = new Dictionary<string, object>()
            {
                { "client_id", appId},
                { "client_secret", appSecret},
                { "grant_type", "client_credentials"}
            };
            IRestResponse response = HttpUtils.MakeRequest(this._linkedInAppSettings.LinkedInAuthorizedUrl, tokenRequestParams);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return string.Empty;

            var tokenModel = JsonConvert.DeserializeObject<LinkedinToken>(response.Content);
            return tokenModel != null ? tokenModel.AccessToken : string.Empty;
        }

        protected override string GetSocialChannelLink() => this._socialMediaSettings.LinkedInLink;

        protected override IRestResponse MakeRequestWithToken(int totalFeeds, string accessToken)
        {
            return null;
        }
    }
}
