using EPiServer.Find;
using EPiServer.Find.ClientConventions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Core.Facet;
using Netafim.WebPlatform.Web.Core.Templates;

/*
 * sources:
 * https://getadigital.com/no/blogg/ascend-15-episerver-find-advanced-developer-session/
 * https://world.episerver.com/forum/developer-forum/EPiServer-Search/Thread-Container/2015/9/epifind---how-to-index-additional-fields/
 * https://andersnordby.wordpress.com/2014/09/19/more-on-indexing-metadata-with-find/
 */

namespace Netafim.WebPlatform.Web.Features.Search
{
    [InitializableModule]
    public class InitializeSearchModule : IConfigurableModule
    {
        private IClient _client;

        public void Initialize(InitializationEngine context)
        {
            _client = context.Locate.Advanced.GetInstance<IClient>();

            _client.Conventions.ForInstancesOf<ICanBeSearched>().IncludeField(x => x.CategoriesFacet());
        }

        public void Uninitialize(InitializationEngine context)
        {
            // do nothing
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            // do nothing
        }
    }
}