using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.GoogleAnalytics;
using Netafim.WebPlatform.Web.Core.Repository;
using Netafim.WebPlatform.Web.Core.Services;
using Netafim.WebPlatform.Web.Features.FloatingCTA;
using Netafim.WebPlatform.Web.Features.Settings.Impl;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace Netafim.WebPlatform.Web.Infrastructure
{
    [InitializableModule]
    [ModuleDependency(typeof(InitializationModule))]
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
            context.Services.AddTransient<IGoogleAnalytics, GoogleAnalytics>();
            context.Services.AddTransient<IEmailSettings, PageDataLayoutSettings>();
            context.Services.AddTransient<IEmailService, SendGridMailService>();
            context.Services.AddTransient<IGoogleAnalyticsSettings, PageDataLayoutSettings>();
            context.Services.AddTransient<IFloatingSharing, PageDataLayoutSettings>();
            context.Services.AddTransient<ICountryRepository, CountryRepository>();
        }
    }
}