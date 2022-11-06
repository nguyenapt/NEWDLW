using System.Collections.Generic;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;

namespace Netafim.WebPlatform.Web.Features.OnThisPage
{
    public class OnThisPageViewModel : IBlockViewModel<OnThisPageBlock>
    {
        public OnThisPageBlock CurrentBlock { get; }

        public IEnumerable<IComponent> Components { get; set; }

        public OnThisPageViewModel(OnThisPageBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
    }
}