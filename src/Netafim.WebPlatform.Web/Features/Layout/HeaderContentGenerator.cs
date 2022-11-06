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
    public class HeaderContentGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;

        public HeaderContentGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator)
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

            var pageRef = EnsureData(context);
            settingsPageCloned.FacebookLink = "https://facebook.com";
            settingsPageCloned.TwitterLink = "https://twitter.com";
            settingsPageCloned.LinkedInLink = "https://linkedin.com";
            settingsPageCloned.YoutubeLink = "https://youtube.com";
            settingsPageCloned.InstagramLink = "https://instagram";

            settingsPageCloned.CarreerLink = pageRef;
            settingsPageCloned.OtherIndustriess = new LinkItemCollection
            {
                new LinkItem() { Text = "Ntf Industry 1", Href = "http://google.com" },
                new LinkItem() { Text = "Ntf Industry 2", Href = "http://lipum.se/" }
            };
            _contentRepository.Save(settingsPageCloned, SaveAction.Publish, AccessLevel.NoAccess);
        }  

        private ContentReference EnsureData(ContentContext context)
        {
            var relatedContentPage = _contentRepository.GetDefault<GenericContainerPage>(context.Homepage);

            relatedContentPage.PageName = "Carreer Page";
            relatedContentPage.URLSegment = _urlSegmentCreator.Create(relatedContentPage);
            relatedContentPage.Title = "Carreer Page";

            return _contentRepository.Save(relatedContentPage, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}