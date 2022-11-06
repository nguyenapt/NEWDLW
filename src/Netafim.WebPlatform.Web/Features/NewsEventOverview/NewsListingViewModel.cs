using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using Netafim.WebPlatform.Web.Features.NewsOverview;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.NewsEventOverview
{
    public class NewsListingViewModel : IBlockViewModel<NewsListingBlock>
    {
        public NewsListingViewModel(NewsListingBlock newsListingBlock)
        {
            CurrentBlock = newsListingBlock;
        }
        public NewsListingBlock CurrentBlock { get; set; }

        public IEnumerable<NewsPage> AllNews { get; set; }
    }
}