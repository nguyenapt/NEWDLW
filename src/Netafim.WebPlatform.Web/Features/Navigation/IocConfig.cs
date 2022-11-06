using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.Navigation.ModelFactories;
using Netafim.WebPlatform.Web.Features.Settings.Impl;

namespace Netafim.WebPlatform.Web.Features.Navigation
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
        }

        private void RegisterSettings(IServiceConfigurationProvider contextServices)
        {
            contextServices.AddHttpContextScoped<INavigationSettings, PageDataLayoutSettings>();
            
            contextServices.AddTransient<IContentGenerator, NavigationContentGenerator>();

            contextServices.AddTransient<IDoormatModelFactory, DoormatTextOnlyModelFactory>();
            contextServices.AddTransient<IDoormatModelFactory, DoormatImageOnlyModelFactory>();
            contextServices.AddTransient<IDoormatModelFactory, DoormatMixedModelFactory>();
            contextServices.AddTransient<IDoormatModelFactory, DoormatEmtypModelFactory>();
            contextServices.AddHttpContextScoped<INavigationRepository, NavigationRepository>();
        }

    }
}