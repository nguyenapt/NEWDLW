using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.Home;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    [ContentGenerator(Order = 10)]
    public class HeroBannerContentGenerator : IContentGenerator
    {
        private const string Thumbnail1 = "~/Features/MediaCarousel/Data/Demo/home-banner.jpg";

        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public HeroBannerContentGenerator(IContentRepository contentRepository,
            ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var homepage = _contentRepository.Get<HomePage>(context.Homepage).CreateWritableClone() as HomePage;

            if (homepage?.Content != null && homepage.Content.FilteredItems.Any(IsMediaCarouselComponent))
            {
                return;
            }
            var assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(homepage.ContentLink);
            var imageCarouselBlock = CreateBlob(assetFolder.ContentLink);

            var containerBlock = _contentRepository.GetDefault<HeroBannerContainerBlock>(assetFolder?.ContentLink);
            ((IContent)containerBlock).Name = "Hero banner";
            containerBlock.Items = new ContentArea();
            containerBlock.Items.Items.Add(new ContentAreaItem
            {
                ContentLink = imageCarouselBlock
            });

            var blockReference = _contentRepository.Save((IContent)containerBlock, SaveAction.Publish, AccessLevel.NoAccess);
            if (homepage.Content == null)
            {
                homepage.Content = new ContentArea();
            }

            homepage.Content.Items.Add(new ContentAreaItem
            {
                ContentLink = blockReference
            });

            _contentRepository.Save(homepage, SaveAction.Publish, AccessLevel.NoAccess);

            context.Homepage = homepage.ContentLink;
        }

        private ContentReference CreateBlob(ContentReference contentRefence)
        {
            var heroBannerBlock = _contentRepository.GetDefault<HeroBannerBlock>(contentRefence);

            ((IContent)heroBannerBlock).Name = "hero block";
            heroBannerBlock.Title = "GROW MORE WITH LESS";
            heroBannerBlock.TextUp = new XhtmlString("<p>Before Netafim (rain-fed)<br> <strong>12 ton/ hectare</strong> of maize</p>");
            heroBannerBlock.TextDown = new XhtmlString("<p>With Netafim irrigations solutions<br> <strong>22 ton/ hectare</strong> and 66% yield <br>increase!</p>");
            heroBannerBlock.Link = new Url("https://www.google.com");
            heroBannerBlock.Image = ((IContent) heroBannerBlock).CreateBlob(Thumbnail1);

            return _contentRepository.Save((IContent)heroBannerBlock, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private bool IsMediaCarouselComponent(ContentAreaItem arg)
        {
            var content = _contentRepository.Get<IContent>(arg.ContentLink);

            return content is HeroBannerContainerBlock;
        }
    }
}