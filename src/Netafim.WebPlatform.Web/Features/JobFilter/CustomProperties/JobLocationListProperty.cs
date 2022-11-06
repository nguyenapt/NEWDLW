using Dlw.EpiBase.Content.Infrastructure.Epi.Shell;
using EPiServer.PlugIn;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;

namespace Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties
{
    [PropertyDefinitionTypePlugIn]
    public class JobLocationListProperty : PropertyListBase<JobLocation> { }
}

