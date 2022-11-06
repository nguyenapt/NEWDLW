using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.Settings.Impl;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{
    [InitializableModule]
    public class IocConfig : IConfigurableModule
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(IocConfig));

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
            context.Services.AddHttpContextScoped<ISystemConfiguratorRepository, DefaultSystemConfiguratorRepository>();
            context.Services.AddHttpContextScoped<ISystemConfiguratorService, DefaultSystemConfiguratorService>();
            context.Services.AddHttpContextScoped<ISystemConfiguratorFlowRateCalculator, ItalySystemConfiguratorFlowRateCalculator>();
            context.Services.AddHttpContextScoped<ISystemConfiguratorProductsRetriever, ItalySystemConfiguratorProductsRetriever>();
            context.Services.AddHttpContextScoped<SystemConfiguratorDbContext, SystemConfiguratorDbContext>();
            context.Services.AddHttpContextScoped<ISystemConfiguratorSettings, PageDataLayoutSettings>();

            // Action handler
            context.Services.AddSingleton<IActionHandler, ActionQueryAdapterBuilder>();
            context.Services.AddSingleton<IActionHandler, ParametersActionQueryBuilder>();

            // Cipher
            context.Services.AddSingleton<ICipher, CaesarCipher>();
        }
    }
}