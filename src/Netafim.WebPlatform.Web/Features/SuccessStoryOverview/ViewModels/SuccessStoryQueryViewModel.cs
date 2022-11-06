using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview.ViewModels
{
    public class SuccessStoryQueryViewModel : QueryViewModel
    {
        public int CropId { get; set; }
        public string Country { get; set; }
        public bool BigProject { get; set; }
        public int CurrentPage { get; set; }
        public bool HasHashData { get; set; }
        public bool FilterWithBoostedDate { get; set; }
    }
}