using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.GenericCTA.Helpers;

namespace Netafim.WebPlatform.Web.Features.GenericCTA
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
            // LinkViewModelFactory
            contextServices.AddTransient<IUrlLinkFactory, NullLinkUrlFactory>();
            contextServices.AddTransient<IUrlLinkFactory, EmailUrlLinkFactory>();
            contextServices.AddTransient<IUrlLinkFactory, InternalLinkUrlFactory>();
            contextServices.AddTransient<IUrlLinkFactory, ExternalLinkUrlFactory>();
            contextServices.AddTransient<IUrlLinkFactory, MediaLinkUrlFactory>();
            contextServices.AddTransient<IUrlLinkFactory, OverlayLinkUrlFactory>();

            // Content generator
            contextServices.AddTransient<IContentGenerator, GenericCTAGenerator>();
        }
    }
}