using System.Collections.Generic;
using DbLocalizationProvider;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview.ViewModels
{
    public class SuccessStoryFilterViewModel : IBlockViewModel<SuccessStoryFilterBlock>
    {
        public SuccessStoryFilterViewModel(SuccessStoryFilterBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
        public SuccessStoryFilterBlock CurrentBlock { get; }
        public Dictionary<int, string> Crops { get; set; }
        public Dictionary<string, string> Countries { get; set; }

        public string RepeatedCriteriaLocalization => LocalizationProvider.Current.GetString(() => Labels.SuccessStoryOverviewRepeatedCriteria);
    }
}