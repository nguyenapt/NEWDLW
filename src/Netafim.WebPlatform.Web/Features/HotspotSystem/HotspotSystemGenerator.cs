using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.Settings;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    public class HotspotSystemGenerator : IContentGenerator
    {
        private const string Image1 = "~/Features/HotspotSystem/Data/Demo/banner-factory.jpg";
        private const string Image2 = "~/Features/HotspotSystem/Data/Demo/banner-field.jpg";
        private const string IconImage = "~/Features/HotspotSystem/Data/Demo/icon-touch.png";

        private readonly IContentRepository _contentRepository;

        public HotspotSystemGenerator(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var hpContainerPage = _contentRepository.GetChildren<HotspotContainerPage>(ContentReference.RootPage).SingleOrDefault();
            if (hpContainerPage != null) return;

            hpContainerPage = _contentRepository.GetDefault<HotspotContainerPage>(ContentReference.RootPage);
            if (hpContainerPage == null) return;

            var homepage = _contentRepository.Get<HomePage>(context.Homepage).CreateWritableClone() as HomePage;

            ((IContent)hpContainerPage).Name = "Hotspot Container";

            var containerPageReference = _contentRepository.Save(hpContainerPage, SaveAction.Publish, AccessLevel.NoAccess);

            var popupTemplate = CreatePopupNode(containerPageReference);
            var linkTemplate = CreateLinkNode(containerPageReference);

            SetHotspostIconFallback();

            if (homepage.Content == null)
            {
                homepage.Content = new ContentArea();
            }

            homepage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = popupTemplate
            });
            homepage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = linkTemplate
            });

            _contentRepository.Save(homepage, SaveAction.Publish, AccessLevel.NoAccess);

            context.Homepage = homepage.ContentLink;
        }

        private ContentReference CreatePopupNode(ContentReference contentReference)
        {
            var popupNodes = new[]
            {
                new {name = "Orchards", x = 49, y = 357, title = "Air valves", subtitle = "Protects the system from severe damage caused by air accumulation and vacum formation"},
                new {name = "Open Field crops", x = 504, y = 230, title = "Air valves", subtitle = "Protects the system from severe damage caused by air accumulation and vacum formation"},
                new {name = "Protected crops", x = 942, y = 58, title = "Air valves", subtitle = "Protects the system from severe damage caused by air accumulation and vacum formation"}
            };

            var hpTemplate = _contentRepository.GetDefault<HotspotPopupPage>(contentReference);
            ((IContent)hpTemplate).Name = "Hotspot Popup Page";
            hpTemplate.Image = hpTemplate.CreateBlob(Image1);
            hpTemplate.ImageWidth = 1440;
            hpTemplate.ImageHeight = 620;
            hpTemplate.HotspotIcon = hpTemplate.CreateBlob(IconImage);

            var hpReference = _contentRepository.Save(hpTemplate, SaveAction.Publish, AccessLevel.NoAccess);

            foreach (var node in popupNodes)
            {
                var hpNode = _contentRepository.GetDefault<HotspotPopupNodePage>(hpReference);

                ((IContent)hpNode).Name = node.name;
                hpNode.NodeName = node.name;
                hpNode.Link = ContentReference.EmptyReference;
                hpNode.CoordinatesX = node.x;
                hpNode.CoordinatesY = node.y;
                hpNode.Title = node.title;
                hpNode.Description = node.subtitle;
                _contentRepository.Save(hpNode, SaveAction.Publish, AccessLevel.NoAccess);
            }

            return hpReference;
        }

        private ContentReference CreateLinkNode(ContentReference contentReference)
        {
            var linkNodes = new[]
            {
                new {name = "Orchards", x = 49, y = 357},
                new {name = "Open Field crops", x = 504, y = 230},
                new {name = "Protected crops", x = 942, y = 58}
            };

            var hpTemplate = _contentRepository.GetDefault<HotspotLinkPage>(contentReference);
            ((IContent)hpTemplate).Name = "Hotspot Clickable Page";
            hpTemplate.Title = "IRRIGATION PRODUCTS SOLUTIONS";
            hpTemplate.SubTitle = "What do you want to irrigate";
            hpTemplate.Image = hpTemplate.CreateBlob(Image2);
            hpTemplate.ImageWidth = 1440;
            hpTemplate.ImageHeight = 620;
            hpTemplate.HotspotIcon = hpTemplate.CreateBlob(IconImage);

            var hpReference = _contentRepository.Save(hpTemplate, SaveAction.Publish, AccessLevel.NoAccess);

            foreach (var node in linkNodes)
            {
                var hpNode = _contentRepository.GetDefault<HotspotLinkNodePage>(hpReference);

                ((IContent)hpNode).Name = node.name;
                hpNode.NodeName = node.name;
                hpNode.Link = ContentReference.EmptyReference;
                hpNode.CoordinatesX = node.x;
                hpNode.CoordinatesY = node.y;
                _contentRepository.Save(hpNode, SaveAction.Publish, AccessLevel.NoAccess);
            }

            return hpReference;
        }

        private void SetHotspostIconFallback()
        {
            var settingsPage = _contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();
            if (settingsPage == null) return;

            var settingsPageCloned = settingsPage.CreateWritableClone() as SettingsPage;
            if (settingsPageCloned != null)
            {
                settingsPageCloned.HotspotIcon = settingsPageCloned.CreateBlob(IconImage);
                _contentRepository.Save(settingsPageCloned, SaveAction.Publish, AccessLevel.NoAccess);
            }
        }
    }
}