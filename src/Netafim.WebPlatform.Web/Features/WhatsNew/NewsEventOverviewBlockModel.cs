using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    public class NewsEventOverviewBlockModel : IBlockViewModel<NewsEventOverviewBlock>
    {
        public NewsEventOverviewBlockModel(NewsEventOverviewBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
        public NewsEventOverviewBlock CurrentBlock { get; set; }       
        public IEnumerable<NewsEventItemViewModel> Items { get; set; }
    }       
}