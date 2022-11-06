using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.Settings;
using System.Linq;
using System;
using Netafim.WebPlatform.Web.Features.Navigation;
using System.Collections.Generic;
using EPiServer.Logging;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public class NavigationContentGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private ILogger _logger = LogManager.GetLogger();

        public NavigationContentGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator)
        {
            _contentRepository = contentRepository;
            _urlSegmentCreator = urlSegmentCreator;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var homepage = _contentRepository.Get<HomePage>(context.Homepage);
            if (homepage == null) { return; }
            var settingsPage = EnsureSettingsPage();
            if (settingsPage == null) return;

            EnsureNavigationRoot(settingsPage, context);
        }

        private void CreateNavigationItemSettings(NavigationContainerPage navigationRoot, ContentReference homePageRef)
        {
            var doormatTypes = new DoormatType[] { DoormatType.TextColumnOnly, DoormatType.ImageColumnOnly, DoormatType.MixedImageAndTextColumn };
            var dictMainNavItem = new Dictionary<DoormatType, ContentReference>();
            foreach (var doormatType in doormatTypes)
            {
                var linkedPageInMainNavItem = CreatePages(homePageRef, doormatType);
                dictMainNavItem.Add(doormatType, linkedPageInMainNavItem);
            }
            foreach (var doormatType in dictMainNavItem.Keys)
            {
                CreateNavigationItemSettingPage(navigationRoot.ContentLink, doormatType, dictMainNavItem[doormatType]);
            }           
        }

        private void CreateNavigationItemSettingPage(ContentReference parent, DoormatType doormatType, ContentReference linkedPageInMainNavItem)
        {
            var navigationPage = _contentRepository.GetDefault<NavigationPage>(parent);
            navigationPage.PageName = GetNameForMainNavItem(doormatType);
            navigationPage.DoormatType = (int)doormatType;
            navigationPage.Link = linkedPageInMainNavItem;
            if (doormatType.Equals(DoormatType.ImageColumnOnly))
            {
                navigationPage.ViewAllText = "All Crops";
            }
            navigationPage.URLSegment = _urlSegmentCreator.Create(navigationPage);
            _contentRepository.Save(navigationPage, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private string GetNameForMainNavItem(DoormatType doormatType)
        {
            return doormatType.Equals(DoormatType.ImageColumnOnly) ? "Crop Expertise"
                  : (doormatType.Equals(DoormatType.TextColumnOnly) ? "Digital farmming" :
                  (doormatType.Equals(DoormatType.MixedImageAndTextColumn) ? "Irrigation products & Solution" : "About"));
        }

        private ContentReference CreatePages(ContentReference parent, DoormatType doormatType)
        {
            var pageNameAtMainMenu = GetNameForMainNavItem(doormatType);
            var pageAtMainNav = CreateSinglePage<GenericContainerPage>(parent, pageNameAtMainMenu);

            var namesForDoormatL1 = new string[] { "Open fields", "Orchards", "Protected Crops" };
            List<ContentReference> pagesAtDoormatL1 = new List<ContentReference>();
            foreach (var pageName in namesForDoormatL1)
            {
                pagesAtDoormatL1.Add(CreateSinglePage<GenericContainerPage>(pageAtMainNav, pageName));

            }

            if (doormatType.Equals(DoormatType.MixedImageAndTextColumn))
            {
                for (int i = 0; i < pagesAtDoormatL1.Count; i++)
                {
                    if (i == 0) continue;
                    var namesForDoormatL2 = new string[] { "Dripplines", "Sprinklers", "Pipe& tumblings" };
                    var namesForDoormatL3 = new string[] { "NetMaize", "NMC Air", "Manage app" };
                    foreach (var pageName in namesForDoormatL2)
                    {
                        var pageAtDoormatL2 = CreateSinglePage<GenericContainerPage>(pagesAtDoormatL1[i], pageName);
                        if (i == 1)
                        {
                            foreach (var pageL3Name in namesForDoormatL3)
                            {
                                CreateSinglePage<GenericContainerPage>(pageAtDoormatL2, pageL3Name);
                            }

                        }
                    }
                }

            }
            return pageAtMainNav;
        }


        private void EnsureNavigationRoot(SettingsPage settingsPage, ContentContext context)
        {
            NavigationContainerPage navigationRoot;
            if (!ContentReference.IsNullOrEmpty(settingsPage.NavigationRoot) && _contentRepository.TryGet(settingsPage.NavigationRoot, out navigationRoot))
            {
                return;
            }

            navigationRoot = _contentRepository.GetDefault<NavigationContainerPage>(ContentReference.RootPage);
            if (navigationRoot == null) return;
            navigationRoot.Name = "Navigation Container";
            navigationRoot.URLSegment = _urlSegmentCreator.Create(navigationRoot);
            var navigationRootRef = _contentRepository.Save(navigationRoot, SaveAction.Publish, AccessLevel.NoAccess);

            var clonedSetingsPage = settingsPage.CreateWritableClone() as SettingsPage;
            clonedSetingsPage.NavigationRoot = navigationRootRef;

            clonedSetingsPage.LocalSites = new[] { new CountryLanguage.LanguageLinkItem() { Language = "en-GB", Link = "netafim.com" } };

            if (_contentRepository.Save(clonedSetingsPage, SaveAction.Publish, AccessLevel.NoAccess) != null)
            {
                CreateNavigationItemSettings(navigationRoot, context.Homepage);

            }
        }

        private SettingsPage EnsureSettingsPage()
        {
            var settingsPage = _contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();
            if (settingsPage != null) return settingsPage;

            var page = _contentRepository.GetDefault<SettingsPage>(ContentReference.RootPage);
            if (_contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess) != null) { return page; };

            return null;
        }

        private ContentReference CreateSinglePage<T>(ContentReference parent, string title) where T : PageData
        {
            var relatedContentPage = _contentRepository.GetDefault<T>(parent);
            relatedContentPage.PageName = title;
            relatedContentPage.URLSegment = _urlSegmentCreator.Create(relatedContentPage);
            return _contentRepository.Save(relatedContentPage, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}