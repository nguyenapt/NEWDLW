using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Core.Rendering
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
            contextServices.AddTransient<IUrlRendering, NullUrlRendering>();
            contextServices.AddTransient<IUrlRendering, RedirectRendering>();
            contextServices.AddTransient<IUrlRendering, OpenNewTabRendering>();
        }
    }
}