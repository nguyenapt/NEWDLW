using Netafim.WebPlatform.Web.Features.Layout;
using Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.ModelFactories
{
    public interface ISocialChannelFeedFactory
    {
        bool IsSatisfied(SocialChannelSetting channelSetting);
        IEnumerable<FeedViewModel> GetFeeds(SocialChannelSetting channelSetting);
        string GetSocialChannelLink();
    }
    public abstract class SocialChannelFeedFactory : ISocialChannelFeedFactory
    {
        protected readonly ISocialMediaSettings _socialMediaSettings;
        protected readonly IEnumerable<ISocialService> _socialServices;
        public SocialChannelFeedFactory(ISocialMediaSettings socialMediaSettings, IEnumerable<ISocialService> socialServices)
        {
            _socialMediaSettings = socialMediaSettings;
            _socialServices = socialServices;
        }
        public virtual IEnumerable<FeedViewModel> GetFeeds(SocialChannelSetting channelSetting)
        {
            var socialService = _socialServices.FirstOrDefault(x => x.IsSatisfied(channelSetting));
            if (socialService == null) throw new System.Exception("Cannot find the satisfied social service.");
            return socialService.GetFeeds(channelSetting.NumberOfFeeds);            
        }

        public abstract bool IsSatisfied(SocialChannelSetting channelSetting);

        public abstract string GetSocialChannelLink();

    }
}
