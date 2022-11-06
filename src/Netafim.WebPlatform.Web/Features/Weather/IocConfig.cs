using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Core.Rest;
using Netafim.WebPlatform.Web.Features.Weather.Awhere;
using System.Net.Http;

namespace Netafim.WebPlatform.Web.Features.Weather
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
            // Action
            context.Services.AddHttpContextScoped<IHttpClientInterceptor, AwhereInterceptor>();
            context.Services.AddHttpContextScoped<AwhereApiOptions, AwhereApiOptions>();
            context.Services.AddHttpContextScoped<IWeatherService, AwhereService>();
            context.Services.AddHttpContextScoped<IWeatherSettings, AwhereApiOptions>();

            context.Services.AddSingleton<AwhereClient, AwhereClient>();
            context.Services.AddTransient<IContentGenerator, WeatherGenerator>();
        }
    }
}