using Castle.Core.Internal;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.WhatsNew.ModelFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    public class SocialFeedsController : BlockController<SocialFeedsBlock>
    {
        private readonly IEnumerable<ISocialChannelFeedFactory> _socialFeedFactories;
        private readonly IContentRepository _contentRepo;
        public SocialFeedsController(IEnumerable<ISocialChannelFeedFactory> socialFeedFactories, IContentRepository contentRepository)
        {
            _socialFeedFactories = socialFeedFactories;
            _contentRepo = contentRepository;
        }
        public override ActionResult Index(SocialFeedsBlock currentBlock)
        {
            IEnumerable<SocialChannelSetting> refinedSocialChannelSettings = GetSocialChannelSettings(currentBlock);
            Dictionary<SocialChannel, string> socialChannelLinks = GetSocialChannelLinks(refinedSocialChannelSettings);
            return PartialView(currentBlock.GetDefaultFullViewName(),
                new SocialFeedsViewModel(currentBlock)
                {
                    SocialChannelLinks = socialChannelLinks
                });
        }

        [HttpGet]
        public virtual ActionResult FetchFeeds(int blockId)
        {
            var socialFeedBlock = _contentRepo.Get<SocialFeedsBlock>(new ContentReference(blockId));          
            IEnumerable<SocialChannelSetting> refinedSocialChannelSettings = GetSocialChannelSettings(socialFeedBlock);
            var feeds = GetSocialFeeds(refinedSocialChannelSettings);
            return PartialView(ResultViewPath(), feeds);
        }
        private  string ResultViewPath()
        {
            return string.Format(Core.Templates.Global.Constants.AbsoluteViewPathFormat, "WhatsNew", "_feeds");
        }

        private Dictionary<SocialChannel, string> GetSocialChannelLinks(IEnumerable<SocialChannelSetting> socialChannelSettings)
        {
            if (socialChannelSettings.IsNullOrEmpty()) return new Dictionary<SocialChannel, string>();
            var result = new Dictionary<SocialChannel, string>();
            foreach (var socialChannelSetting in socialChannelSettings)
            {
                var feedFactory = _socialFeedFactories.FirstOrDefault(x => x.IsSatisfied(socialChannelSetting));
                if (feedFactory == null) throw new Exception("Cannot find the satisfied social channel factory!");
                result.Add(socialChannelSetting.Channel, feedFactory.GetSocialChannelLink());
            }
            return result;
        }

        private IEnumerable<FeedViewModel> GetSocialFeeds(IEnumerable<SocialChannelSetting> socialChannelSettings)
        {
            if (socialChannelSettings.IsNullOrEmpty()) return Enumerable.Empty<FeedViewModel>();

            var result = new List<FeedViewModel>();
            foreach (var socialChannelSetting in socialChannelSettings)
            {
                var feedFactory = _socialFeedFactories.FirstOrDefault(x => x.IsSatisfied(socialChannelSetting));
                if (feedFactory == null) throw new Exception("Cannot find the satisfied social channel factory!");
                var feeds = feedFactory.GetFeeds(socialChannelSetting);
                if (feeds.IsNullOrEmpty()) continue;
                result.AddRange(feeds);
            }
            return result;
        }

        private IEnumerable<SocialChannelSetting> GetSocialChannelSettings(SocialFeedsBlock currentBlock)
        {
            if (currentBlock == null || currentBlock.SocialChannelsSettings.IsNullOrEmpty()) return Enumerable.Empty<SocialChannelSetting>();
            var channels = currentBlock.SocialChannelsSettings.Select(x => x.Channel).Distinct();
            var result = new List<SocialChannelSetting>();
            foreach (var channel in channels)
            {
                result.Add(currentBlock.SocialChannelsSettings.FirstOrDefault(x => x.Channel.Equals(channel)));
            }
            return result;
        }
    }
}