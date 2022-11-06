using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.Settings.Impl;
using static Netafim.WebPlatform.Web.Features.Layout.LinkItemMapper;

namespace Netafim.WebPlatform.Web.Features.Layout
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
            contextServices.AddHttpContextScoped<IHeaderSettings, PageDataLayoutSettings>();
            contextServices.AddHttpContextScoped<IFooterSettings, PageDataLayoutSettings>();
            contextServices.AddHttpContextScoped<ISocialMediaSettings, PageDataLayoutSettings>();
            contextServices.AddHttpContextScoped<ILinkItemMapper, LinkItemMapper>();
            contextServices.AddHttpContextScoped<IFooterRepository, FooterRepository>();

            // LinkViewModelFactory
            contextServices.AddTransient<ILinkViewModelFactory,  EmaiLinkViewModelFactory>();
            contextServices.AddTransient<ILinkViewModelFactory,  ExternalLinkViewModelFactory>();
            contextServices.AddTransient<ILinkViewModelFactory,  InternalLinkViewModelFactory>();

            contextServices.AddTransient<IContentGenerator, HeaderContentGenerator>();
            contextServices.AddTransient<IContentGenerator, FooterContentGenerator>();
        }
    }
}