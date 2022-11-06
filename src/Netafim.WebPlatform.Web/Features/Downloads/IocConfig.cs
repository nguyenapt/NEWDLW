using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.Settings.Impl;

namespace Netafim.WebPlatform.Web.Features.Downloads
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
            contextServices.AddTransient<IContentGenerator, DownloadsGenerator>();
        }

    }
}