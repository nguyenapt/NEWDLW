using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services
{
    public interface ISystemConfiguratorFlowRateCalculator
    {
        decimal Calculate(SystemConfiguratorFlowRateData data);
    }
}