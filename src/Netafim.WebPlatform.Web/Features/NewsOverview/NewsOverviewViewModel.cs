using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.NewsOverview
{
    public class NewsOverviewViewModel : IBlockViewModel<NewsOverviewBlock>
    {
        public NewsOverviewViewModel(NewsOverviewBlock newsOverviewBlock)
        {
            CurrentBlock = newsOverviewBlock;
        }
        public NewsOverviewBlock CurrentBlock { get; set; }

        public IEnumerable<int> Years { get; set; }

        public int LatestYearHavingNews { get; set; } 
    }
}