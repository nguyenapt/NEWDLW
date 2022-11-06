using Netafim.WebPlatform.Web.Features.Layout;
using Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.ModelFactories
{
    public class TwitterFeedFactory : SocialChannelFeedFactory
    {
        public TwitterFeedFactory(ISocialMediaSettings socialMediaSettings, IEnumerable<ISocialService> socialServices) : base(socialMediaSettings, socialServices)
        {
        }

        public override string GetSocialChannelLink()
        {
            return _socialMediaSettings.TwitterLink;
        }

        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting.Channel.Equals(SocialChannel.Twitter);
        }
    }
}