using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Features.Settings;

namespace Netafim.WebPlatform.Web.Features.FloatingCTA
{
    public class FloatingCtaContentGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;

        public FloatingCtaContentGenerator(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public void Generate(ContentContext context)
        {
            AddScriptToPageSettins();
        }

        private void AddScriptToPageSettins()
        {
            var settingPage = EnsureSettingsPage().CreateWritableClone() as SettingsPage;
            if (settingPage == null)
                return;

            settingPage.ShareaholicSiteId = "d8cd6430751aa03657fc2868bba41191";
            _contentRepository.Save(settingPage, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private SettingsPage EnsureSettingsPage()
        {
            var settingsPage = _contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();
            if (settingsPage != null) return settingsPage;

            var page = _contentRepository.GetDefault<SettingsPage>(ContentReference.RootPage);
            if (_contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess) != null) { return page; };

            return null;
        }
    }
}