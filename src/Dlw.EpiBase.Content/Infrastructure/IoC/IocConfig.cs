using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Dlw.EpiBase.Content.Infrastructure.IoC
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))] // for DependencyResolver.
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
            // configure mvc's dependency resolver
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.StructureMap()));
        }
    }
}