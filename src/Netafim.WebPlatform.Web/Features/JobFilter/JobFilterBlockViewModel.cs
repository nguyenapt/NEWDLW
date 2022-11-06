using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public class JobFilterBlockViewModel : IBlockViewModel<JobFilterBlock>
    {
        public JobFilterBlockViewModel(JobFilterBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
        public JobFilterBlock CurrentBlock { get; set; }
        public Dictionary<string, string> Departments { get; set; }
        public Dictionary<string, string> Locations { get; set; }


    }
}