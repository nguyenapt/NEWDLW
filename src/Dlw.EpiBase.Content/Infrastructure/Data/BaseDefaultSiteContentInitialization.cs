using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

// Code inspired on DefaultSiteContentInitialization
namespace Dlw.EpiBase.Content.Infrastructure.Data
{
    /// <summary>
    /// Run after DefaultSiteContentInitialization, when dev decides to use import from other environment, own export, default init should be skipped 
    /// </summary>
    /// <remarks>
    /// Flag implementation with the following attribute: [ModuleDependency(typeof(EPiServer.Enterprise.Internal.DefaultSiteContentInitialization))]
    /// </remarks>
    public abstract class BaseDefaultSiteContentInitialization : IInitializableHttpModule
    {
        private static readonly object _lock = new object();
        private static bool _generateDefaultContent;

        public void Initialize(InitializationEngine context)
        {
            var siteDefinitionRepository = context.Locate.Advanced.GetInstance<ISiteDefinitionRepository>();
            var contentLoader = context.Locate.Advanced.GetInstance<IContentLoader>();
            var appSettings = context.Locate.Advanced.GetInstance<IAppSettings>();

            _generateDefaultContent = context.HostType == HostType.WebApplication &&
                            HostingEnvironment.IsHosted &&
                            !siteDefinitionRepository.List().Any() &&
                            contentLoader.GetChildren<PageData>(ContentReference.RootPage).All(p => p.ContentLink == ContentReference.WasteBasket) &&
                            !appSettings.IsProduction;

            EnsureSiteConfiguration(context.Locate.Advanced);
        }

        public void Uninitialize(InitializationEngine context)
        {
            // do nothing
        }

        public void InitializeHttpEvents(HttpApplication application)
        {
            //if (!_generateDefaultContent)
            //    return;

            application.BeginRequest += ApplicationOnBeginRequest;
        }

        /// <summary>
        /// Implement to provision required site configuration.
        /// </summary>
        /// <param name="serviceLocator"></param>
        protected abstract void EnsureSiteConfiguration(IServiceLocator serviceLocator);

        /// <summary>
        /// Implement to provision default site with content.
        /// Will only be triggered when there are no other sites defined and there are no content under the root page beside the wastebasket.
        /// </summary>
        /// <returns>Returns start page.</returns>
        /// <param name="serviceLocator"></param>
        protected abstract ContentReference CreateDefaultContent(IServiceLocator serviceLocator);

        private void ApplicationOnBeginRequest(object sender, EventArgs eventArgs)
        {
            var serviceLocator = ServiceLocator.Current;

            if (!_generateDefaultContent)
                return;

            lock (_lock)
            {
                if (!_generateDefaultContent)
                    return;

                var startPage = CreateDefaultContent(serviceLocator);
                CreateSiteDefinition(serviceLocator, startPage);

                _generateDefaultContent = false;
            }
        }

        private static void CreateSiteDefinition(IServiceLocator serviceLocator, ContentReference startPage)
        {
            var siteDefinitionRepository = serviceLocator.GetInstance<ISiteDefinitionRepository>();

            var siteDefinition = new SiteDefinition
            {
                StartPage = startPage,
                Name = HostingEnvironment.SiteName
            };

            var urlBuilder = new UrlBuilder(HttpContext.Current.Request.Url)
            {
                Path = WebHostingEnvironment.Instance.WebRootVirtualPath
            };

            siteDefinition.SiteUrl = new Uri(VirtualPathUtility.AppendTrailingSlash((string)urlBuilder));
            siteDefinition.Hosts.Add(new HostDefinition()
            {
                Name = urlBuilder.Uri.Authority
            });

            siteDefinition.Hosts.Add(new HostDefinition()
            {
                Name = "*"
            });

            siteDefinitionRepository.Save(siteDefinition);

            SiteDefinition.Current = siteDefinition;
        }

    }
}