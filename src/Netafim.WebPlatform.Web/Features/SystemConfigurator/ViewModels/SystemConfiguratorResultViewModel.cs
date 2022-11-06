using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.ViewModels
{
    public class SystemConfiguratorResultViewModel
    {
        public SystemConfiguratorResult Result { get; set; }

        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string SubmissionId { get; set; }

        public IList<ParameterGroupSettingsViewModel> Settings { get; set; }

        public IList<string> Messages { get; set; }

        public SystemConfiguratorResultViewModel()
        {
            Messages = new List<string>();
        }
    }

    public class ParameterSettingViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ParameterGroupSettingsViewModel : List<ParameterSettingViewModel>
    {
        public ParameterGroupSettingsViewModel(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}