using Netafim.WebPlatform.Web.Core.Extensions;
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

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public class FooterContentGenerator : IContentGenerator
    {
        private const string DataDemoFolder = @"~/Features/Layout/Data/Demo/{0}";
        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;

        public FooterContentGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator)
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
            var homepage = _contentRepository.Get<HomePage>(context.Homepage).CreateWritableClone() as HomePage;
            if (homepage == null) { return; }

            var settingsPage = _contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();
            if (settingsPage == null) return;

            var settingsPageCloned = settingsPage.CreateWritableClone() as SettingsPage;

            settingsPageCloned.ContatUs = EnsureData(context);
            settingsPageCloned.Logo = homepage.CreateBlob(string.Format(DataDemoFolder, "white-logo.png"));
            
            settingsPageCloned.SubFooter = new LinkItemCollection
            {
                new LinkItem() { Text = "NETAFIM", Href = "http://NETAFIM.com" },
                new LinkItem() { Text = "TERMS & CONDITIONS", Href = "http://lipum.se/" },
                new LinkItem() { Text = "PRIVACY POLICY", Href = "http://lipum.se/"}
            };
            _contentRepository.Save(settingsPageCloned, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private ContentReference EnsureData(ContentContext context)
        {
            var relatedContentPage = _contentRepository.GetDefault<GenericContainerPage>(context.Homepage);

            relatedContentPage.PageName = "Contact Page";
            relatedContentPage.URLSegment = _urlSegmentCreator.Create(relatedContentPage);
            relatedContentPage.Title = "Contact Page";

            return _contentRepository.Save(relatedContentPage, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}