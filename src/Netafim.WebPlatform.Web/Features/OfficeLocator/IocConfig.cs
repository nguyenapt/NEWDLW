using Netafim.WebPlatform.Web.Features.Settings.Impl;
using Netafim.WebPlatform.Web.Features.OfficeLocator.Services;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator
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
            context.Services.AddTransient<IOfficeService, OfficeService>();
            context.Services.AddHttpContextScoped<IOfficeSettings, PageDataSearchSettings>();
            context.Services.AddHttpContextScoped<IGoogleSettings, PageDataSearchSettings>();

            context.Services.AddTransient<IContentGenerator, OfficeLocatorGenerator>();
        }
    }
}