using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public class JobFilterBlockQueryViewModel : QueryViewModel
    {
        public string Department { get; set; }
        public string Location { get; set; }
    }
}