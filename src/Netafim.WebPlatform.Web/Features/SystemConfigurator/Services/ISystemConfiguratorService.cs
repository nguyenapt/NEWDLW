using System.Globalization;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services
{
    public interface ISystemConfiguratorService
    {
        SystemConfiguratorResult Process(SystemConfiguratorData data, CultureInfo culture);
    }
}
