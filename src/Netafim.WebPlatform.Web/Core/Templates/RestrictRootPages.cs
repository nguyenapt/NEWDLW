using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.HotspotSystem;
using Netafim.WebPlatform.Web.Features.Navigation;
using Netafim.WebPlatform.Web.Features.OfficeLocator;
using Netafim.WebPlatform.Web.Features.Settings;
using Netafim.WebPlatform.Web.Infrastructure.Initialization;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    [InitializableModule]
    [ModuleDependency(typeof(NtfmSiteContentInitialization))]
    public class RestrictRootPages : IInitializableModule
    {
        /// <summary>
        /// Restrict available page types for the root page. 
        /// If these settings are edited in admin mode, these will be overridden on initialization. 
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationEngine context)
        {
            var contentTypeRepository = context.Locate.Advanced.GetInstance<IContentTypeRepository>();
            var availabilityRepository = context.Locate.Advanced.GetInstance<IAvailableSettingsRepository>();

            var sysRoot = contentTypeRepository.Load("SysRoot") as PageType;

            var homePage = contentTypeRepository.Load(typeof(HomePage));
            var contentFolder = contentTypeRepository.Load(typeof(ContentFolder));
            var settingsPage = contentTypeRepository.Load(typeof(SettingsPage));
            var hotspotContainerPage = contentTypeRepository.Load(typeof(HotspotContainerPage));
            var navigationContainerPage = contentTypeRepository.Load(typeof(NavigationContainerPage));
            var officeLocatorContainerPage = contentTypeRepository.Load(typeof(OfficeLocatorContainerPage));
            var noTemplateContainerPage = contentTypeRepository.Load(typeof(NoTemplateContainerPage));

            var setting = new AvailableSetting { Availability = Availability.Specific };
            setting.AllowedContentTypeNames.Add(contentFolder.Name);
            setting.AllowedContentTypeNames.Add(homePage.Name);
            setting.AllowedContentTypeNames.Add(settingsPage.Name);
            setting.AllowedContentTypeNames.Add(hotspotContainerPage.Name);
            setting.AllowedContentTypeNames.Add(navigationContainerPage.Name);
            setting.AllowedContentTypeNames.Add(officeLocatorContainerPage.Name);
            setting.AllowedContentTypeNames.Add(noTemplateContainerPage.Name);

            availabilityRepository.RegisterSetting(sysRoot, setting);
        }

        public void Uninitialize(InitializationEngine context)
        {
            // do nothing, can not rollback
        }
    }
}