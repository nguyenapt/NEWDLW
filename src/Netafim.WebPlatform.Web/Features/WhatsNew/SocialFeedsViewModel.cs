using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    public class SocialFeedsViewModel : IBlockViewModel<SocialFeedsBlock>
    {
        public SocialFeedsViewModel( SocialFeedsBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
        public SocialFeedsBlock CurrentBlock { get; set; }        
        public Dictionary<SocialChannel, string> SocialChannelLinks { get; set; }
    }       
}