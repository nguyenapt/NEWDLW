using System.Collections.Generic;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.CropsOverview;
using Netafim.WebPlatform.Web.Features.SuccessStory;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview.ViewModels
{
    public class SuccessStoryListingViewModel : PaginableBlockViewModel<SuccessStoryFilterBlock, SuccessStoryPage>
    {
        public IEnumerable<CropsPage> CropPages { get; set; }

        public SuccessStoryListingViewModel(SuccessStoryFilterBlock currentBlock, IPagedList<SuccessStoryPage> result, IEnumerable<CropsPage> cropPages) 
            : base(currentBlock, result)
        {
            CropPages = cropPages;
        }
    }
}