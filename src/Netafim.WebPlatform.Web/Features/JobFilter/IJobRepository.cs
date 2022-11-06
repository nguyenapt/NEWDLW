using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public interface IJobRepository
    {
        Dictionary<string, string> GetAllDepartments();
        Dictionary<string, string> GetAllLocations();
        Dictionary<string, string> GetAllPositions();

        JobDepartment GetDepartment(string key);
        JobPosition GetPosition(string key);
    }
}