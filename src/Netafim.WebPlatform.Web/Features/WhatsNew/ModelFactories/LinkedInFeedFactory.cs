using Netafim.WebPlatform.Web.Features.Layout;
using Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.ModelFactories
{
    public class LinkedInFeedFactory : SocialChannelFeedFactory
    {
        public LinkedInFeedFactory(ISocialMediaSettings socialMediaSettings, IEnumerable<ISocialService> socialServices) : base(socialMediaSettings, socialServices)
        {
        }
        public override IEnumerable<FeedViewModel> GetFeeds(SocialChannelSetting channelSetting)
        {
            return Enumerable.Empty<FeedViewModel>();
        }

        public override string GetSocialChannelLink()
        {
            return _socialMediaSettings.LinkedInLink;
        }
        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting.Channel.Equals(SocialChannel.LinkedIn);
        }
    }
}