using System.Collections.Generic;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;

namespace Netafim.WebPlatform.Web.Features.OnThisLevel
{
    public class OnThisLevelViewModel : IBlockViewModel<OnThisLevelBlock>
    {
        public OnThisLevelBlock CurrentBlock { get; }

        public IEnumerable<PageBase> SiblingPages { get; set; }

        public OnThisLevelViewModel(OnThisLevelBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
    }
}