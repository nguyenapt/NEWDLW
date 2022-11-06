using Dlw.EpiBase.Content;
using Dlw.EpiBase.Content.Infrastructure.Epi;
using Dlw.EpiBase.Content.Infrastructure.Seo;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Features.Settings.Impl;
using Netafim.WebPlatform.Web.Infrastructure.Owin.Security;
using Netafim.WebPlatform.Web.Infrastructure.Settings;

namespace Netafim.WebPlatform.Web
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
            RegisterSettings(context.Services);

            RegisterEpiCustomizations(context.Services);
        }

        private void RegisterSettings(IServiceConfigurationProvider contextServices)
        {
            contextServices.AddSingleton<IAppSettings, ConfigurationManagerAppSettings>();
            contextServices.AddHttpContextScoped<ILayoutSettings, PageDataLayoutSettings>();
            contextServices.AddHttpContextScoped<ISeoSettings, PageDataSeoSettings>();
            contextServices.AddSingleton<ISecurityShieldSettings, ConfigurationManagerSecurityShieldSettings>();
        }

        private void RegisterEpiCustomizations(IServiceConfigurationProvider contextServices)
        {
            // can not be registered in Dlw.EpiBase.Content.IocConfig, too early, gets overriden by default implementation
            contextServices.AddSingleton<IContentRenderer, ErrorHandlingContentRenderer>();
        }
    }
}