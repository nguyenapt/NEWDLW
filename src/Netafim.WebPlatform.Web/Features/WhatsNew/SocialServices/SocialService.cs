using Netafim.WebPlatform.Web.Core.Caching;
using Netafim.WebPlatform.Web.Features.Layout;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public abstract class SocialService : ISocialService
    {
        protected readonly ICacheProvider _cacheManager;
        protected readonly string _socialName = string.Empty;
        protected int _totalItems;
        protected readonly ISocialMediaSettings _socialMediaSettings;

        protected SocialService(ICacheProvider cacheManager, ISocialMediaSettings socialMediaSettings)
        {
            _cacheManager = cacheManager;
            _socialMediaSettings = socialMediaSettings;
            _socialName = GetSocialChannelName();
        }

        private string GetSocialChannelName()
        {
            var fbUrl = GetSocialChannelLink();
            if (string.IsNullOrWhiteSpace(fbUrl)) { return GetDefaultChannelName(); }

            var regex = new Regex(@"/+\s*[^\s/]+[\s/]*$");
            var match = regex.Match(fbUrl);

            if (match == null || !match.Success || string.IsNullOrEmpty(match.Value)) return GetDefaultChannelName();
            var fbNameRegex = new Regex(@"[^\s/]+");
            return fbNameRegex.Match(match.Value).Value;
        }

        public IEnumerable<FeedViewModel> GetFeeds(int totalFeeds)
        {
            var accessToken = GetAccessToken();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Enumerable.Empty<FeedViewModel>();
            }
            _totalItems = totalFeeds;

            return GetFeedsWithToken(totalFeeds, accessToken);
        }

        private IEnumerable<FeedViewModel> GetFeedsWithToken(int totalFeeds, string accessToken)
        {
            var emptyResult = Enumerable.Empty<FeedViewModel>();
            if (string.IsNullOrEmpty(accessToken)) return emptyResult;

            var response = MakeRequestWithToken(totalFeeds, accessToken);
            if (response == null) return emptyResult;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // retry to get the new token.
                // remove the existing token.
                _cacheManager.Remove(GetAccessTokenCacheKey());

                //refresh the token.
                var newToken = GetNewAccessToken();
                if (string.IsNullOrWhiteSpace(newToken)) { return emptyResult; }
                if (string.IsNullOrWhiteSpace(newToken)) { _cacheManager.Add(GetAccessTokenCacheKey(), newToken, GetAccessTokenCacheOptions()); }

                // retry to get feed with the new token.
                response = MakeRequestWithToken(totalFeeds, newToken);
                if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK) { return emptyResult; }
            }
            return ExtractFeedViewModels(response);
        }

        protected abstract IEnumerable<FeedViewModel> ExtractFeedViewModels(IRestResponse apiResponse);

        private string GetAccessToken()
        {
            string tokenFromCache;
            if (_cacheManager.TryGetValue<string>(GetAccessTokenCacheKey(), out tokenFromCache))
            {
                if (!string.IsNullOrWhiteSpace(tokenFromCache)) { return tokenFromCache; }
            }
            _cacheManager.Remove(GetAccessTokenCacheKey());
            var newToken = GetNewAccessToken();
            if (!string.IsNullOrWhiteSpace(newToken))
            {
                _cacheManager.Add(GetAccessTokenCacheKey(), newToken, GetAccessTokenCacheOptions());
            }
            return newToken;
        }

        protected abstract IRestResponse MakeRequestWithToken(int totalFeeds, string accessToken);
        protected abstract CacheOptions GetAccessTokenCacheOptions();
        protected abstract CacheOptions GetFeedsCacheOptions();
        protected abstract string GetFeedsCacheKey();
        protected abstract string GetNewAccessToken();
        protected abstract string GetAccessTokenCacheKey();
        protected abstract string GetSocialChannelLink();
        protected abstract string GetDefaultChannelName();

        public abstract bool IsSatisfied(SocialChannelSetting channelSetting);
    }
}