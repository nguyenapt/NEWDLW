using System.Collections.Generic;
using Netafim.WebPlatform.Web.Core.Caching;
using Netafim.WebPlatform.Web.Features.Layout;
using RestSharp;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class InstagramService : SocialService
    {
        public InstagramService(ICacheProvider cacheManager, ISocialMediaSettings socialMediaSettings) : base(cacheManager, socialMediaSettings)
        {
        }

        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting != null && channelSetting.Channel == SocialChannel.Instagram;
        }      

        protected override IEnumerable<FeedViewModel> ExtractFeedViewModels(IRestResponse apiResponse)
        {
            return null;
        }

        protected override string GetAccessTokenCacheKey() => string.Empty;

        protected override CacheOptions GetAccessTokenCacheOptions() => null;

        protected override string GetDefaultChannelName() => string.Empty;

        protected override string GetFeedsCacheKey() => string.Empty;

        protected override CacheOptions GetFeedsCacheOptions() => null;

        protected override string GetNewAccessToken() => string.Empty;

        protected override string GetSocialChannelLink() => string.Empty;

        protected override IRestResponse MakeRequestWithToken(int totalFeeds, string accessToken)
        {
            return null;
        }
    }
}
