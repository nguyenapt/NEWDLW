using System.Collections.Generic;
using Netafim.WebPlatform.Web.Features.Layout;
using Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.ModelFactories
{
    public class YoutubeFeedFactory : SocialChannelFeedFactory
    {
        public YoutubeFeedFactory(ISocialMediaSettings socialMediaSettings, IEnumerable<ISocialService> socialServices) : base(socialMediaSettings, socialServices)
        {
        }

        public override string GetSocialChannelLink()
        {
            return _socialMediaSettings.YoutubeLink;
        }

        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting.Channel.Equals(SocialChannel.Youtube);
        }
    }
}