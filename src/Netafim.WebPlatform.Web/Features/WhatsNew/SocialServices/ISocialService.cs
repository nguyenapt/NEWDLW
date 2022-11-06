using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public interface ISocialService
    {
        IEnumerable<FeedViewModel> GetFeeds(int totalFeeds);
        bool IsSatisfied(SocialChannelSetting channelSetting);
    }
}
