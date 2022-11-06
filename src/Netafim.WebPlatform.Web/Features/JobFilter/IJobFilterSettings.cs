using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public interface IJobFilterSettings
    {
        IList<JobDepartment> JobDepartments { get; }
        IList<JobLocation> JobLocations { get; }
        IList<JobPosition> JobPositions { get; }
    }
}
