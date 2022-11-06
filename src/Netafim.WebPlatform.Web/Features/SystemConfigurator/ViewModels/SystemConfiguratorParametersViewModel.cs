using System.Collections.Generic;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.ViewModels
{
    public class SystemConfiguratorParametersViewModel : IBlockViewModel<SystemConfiguratorParametersBlock>
    {
        public SystemConfiguratorParametersBlock CurrentBlock { get; }

        public SystemConfiguratorParametersViewModel(SystemConfiguratorParametersBlock block)
        {
            this.CurrentBlock = block;
        }
        public IEnumerable<ParameterGroupViewModel> ParameterGroups { get; set; }

        public IEnumerable<Crop> Crops { get; set; }

        public IEnumerable<Region> Regions { get; set; }

        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string SubmissionId { get; set; }

        // TODO other values inc possible values / ranges / validation

        public string ActionUrl { get; set; }
    }
}