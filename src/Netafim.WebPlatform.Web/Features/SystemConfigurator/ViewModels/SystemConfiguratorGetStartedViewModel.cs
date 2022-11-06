using System.ComponentModel.DataAnnotations;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.ViewModels
{
    public class SystemConfiguratorGetStartedViewModel : IBlockViewModel<SystemConfiguratorGetStarterBlock>
    {
        public SystemConfiguratorGetStartedViewModel(SystemConfiguratorGetStarterBlock currentBlock)
        {
            this.CurrentBlock = currentBlock;
        }
        
        public string ActionUrl { get; set; }

        public SystemConfiguratorGetStarterBlock CurrentBlock { get; }
    }
}