using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
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
            context.Services.AddTransient<IContentGenerator, MediaCarouselContentGenerator>();
            context.Services.AddTransient<IContentGenerator, HeroBannerContentGenerator>();

            context.Services.AddTransient<IUrlLinkFactory, SuccessPageUrlLinkFactory>();
            context.Services.AddTransient<IUrlLinkFactory, MediaCarouselUrlLinkFactory>();

            context.Services.AddTransient<ICarouselContainerModeFactory, CarouselBoxedModeFactory>();
            context.Services.AddTransient<ICarouselContainerModeFactory, CarouselFullWidthModeFactory>();
        }
    }
}