using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using EPiServer;
using EPiServer.Configuration;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    [ModuleDependency(typeof(RestrictRootPages))]
    public class BynderInitializer : IInitializableModule, IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var settings = context.Locate.Advanced.GetInstance<IBynderSettings>();

            if (!settings.Enabled) return;

            var bynderRoot = EnsureCreateProviderRoot(context, settings);

            EnsureProvider(context, bynderRoot);

            EnsureRoutes(context, bynderRoot);

            // Warmup disabled for the moment, provider is not fully initialized, throws exception
            WarmUp(context);
        }

        private void EnsureRoutes(InitializationEngine context, IContent bynderRoot)
        {
            // Since we have our structure outside asset root we registera custom route for it
            // and remove the language segment
            // -> disabled for the moment, we want content to be localizable
            //RouteTable.Routes.MapContentRoute(
            //name: "BynderMedia",
            //    url: "bynder/{node}/{partial}/{action}",
            //    defaults: new { action = "index" },
            //    contentRootResolver: (s) => bynderRoot.ContentLink);

            // EPiServer UI needs the language segment

            RouteTable.Routes.MapContentRoute(
                name: "BynderMediaEdit",
                url: GetCmsHomePath(context.Locate) + "bynder/{language}/{medianodeedit}/{partial}/{action}",
                defaults: new { action = "index" },
                contentRootResolver: (s) => bynderRoot.ContentLink);
        }

        private static string GetCmsHomePath(ServiceLocationHelper serviceLocation)
        {
            var virtualPathResolver = serviceLocation.Advanced.GetInstance<IVirtualPathResolver>();

            var cmsContentPath = VirtualPathUtility.AppendTrailingSlash(EPiServer.Shell.Paths.ToResource("CMS", "Content"));
            return virtualPathResolver.ToAppRelative(cmsContentPath).Substring(1);
        }

        private IContent EnsureCreateProviderRoot(InitializationEngine context, IBynderSettings settings)
        {
            var contentRepository = context.Locate.ContentRepository();

            var bynderRoot = contentRepository.GetBySegment(SiteDefinition.Current.RootPage, settings.ProviderName, LanguageSelector.AutoDetect(true));

            if (bynderRoot == null)
            {
                bynderRoot = contentRepository.GetDefault<ContentFolder>(SiteDefinition.Current.RootPage);
                bynderRoot.Name = settings.ProviderName;
                contentRepository.Save(bynderRoot, SaveAction.Publish, AccessLevel.NoAccess);
            }

            return bynderRoot;
        }

        private void EnsureProvider(InitializationEngine context, IContent bynderRoot)
        {
            var contentProviderManager = context.Locate.Advanced.GetInstance<IContentProviderManager>();
            
            var configValues = new NameValueCollection
            {
                {ContentProviderElement.EntryPointString, bynderRoot.ContentLink.ToString() },
                {ContentProviderElement.CapabilitiesString, "Search" }
            };
            var provider = context.Locate.Advanced.GetInstance<BynderProvider>();

            provider.Initialize(BynderProvider.Key, configValues);
            contentProviderManager.ProviderMap.AddProvider(provider);
        }

        private void WarmUp(InitializationEngine context)
        {
            context.InitComplete += delegate(object sender, EventArgs args)
            {
                var contentProviderManager = ServiceLocator.Current.GetInstance<IContentProviderManager>();
                var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

                var provider = contentProviderManager.GetProvider(BynderProvider.Key);
                contentRepository.GetChildren<MediaData>(provider.EntryPoint.ToPageReference());
            };
        }

        public void Uninitialize(InitializationEngine context)
        {
            // do nothing
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IBynderSettings, ConfigurationManagerBynderSettings>();
            context.Services.AddSingleton<IBynderRepository, DefaultBynderRepository>();
            context.Services.AddSingleton<IAssetConverter, DefaultAssetConverter>();
            context.Services.AddSingleton<IAssetMappedIdentityBuilder, DefaultAssetMappedIdentityBuilder>();
        }
    }
}