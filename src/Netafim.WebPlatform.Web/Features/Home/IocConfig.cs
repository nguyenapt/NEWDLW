using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Features.Home
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
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
            context.Services.AddTransient<IContentGenerator, HomePageContentGenerator>();
        }
    }
}