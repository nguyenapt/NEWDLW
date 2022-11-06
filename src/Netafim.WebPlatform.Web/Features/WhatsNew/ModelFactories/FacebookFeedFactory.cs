using Netafim.WebPlatform.Web.Features.Layout;
using Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices;
using System;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.ModelFactories
{
    public class FacebookFeedFactory : SocialChannelFeedFactory
    {
        public FacebookFeedFactory(ISocialMediaSettings socialSettings, IEnumerable<ISocialService> socialServices) 
            : base(socialSettings, socialServices)
        {
        }       

        public override bool IsSatisfied(SocialChannelSetting channelSetting)
        {
            return channelSetting.Channel.Equals(SocialChannel.Facebook);
        }

        public override string GetSocialChannelLink()
        {
            return _socialMediaSettings.FacebookLink;
        }
    }
}