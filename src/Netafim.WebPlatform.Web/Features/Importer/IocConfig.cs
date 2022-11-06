using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter;

namespace Netafim.WebPlatform.Web.Features.Importer
{
    [InitializableModule]
    public class IocConfig : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            // do nothing
        }

        public void Uninitialize(InitializationEngine context)
        {
            // do nothing
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ISystemConfiguratorImporter, XmlSystemConfiguratorImporter>();
        }
    }
}