using System.Collections.Generic;
using EPiServer;
using Netafim.WebPlatform.Web.Features.JobFilter;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;

namespace Netafim.WebPlatform.Web.Features.Settings.Impl
{
    public class PageJobSettings : PageDataSettings, IJobFilterSettings
    {
        public PageJobSettings(IContentRepository contentRepository) : base(contentRepository)
        {
        }
        public IList<JobDepartment> JobDepartments => SettingsPage.JobDepartments;

        public IList<JobLocation> JobLocations => SettingsPage.JobLocations;

        public IList<JobPosition> JobPositions => SettingsPage.JobPositions;
    }
}