using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.MediaCarousel;

namespace Netafim.WebPlatform.Web.Features.SuccessStory
{
    [ContentGenerator(Order = 60)]
    public class SuccessStoryContentGenerator : IContentGenerator
    {
        private const string Thumbnail1 = "~/Features/MediaCarousel/Data/Demo/farm1.jpg";
        private const string Thumbnail2 = "~/Features/MediaCarousel/Data/Demo/testimonial_senegal.png";

        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public SuccessStoryContentGenerator(IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
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
            var assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(context.Homepage);

            if (homepage == null)
            {
                return;
            }

            var containerBlock = _contentRepository.GetDefault<MediaCarouselContainerBlock>(assetFolder.ContentLink);
            ((IContent)containerBlock).Name = "Success Story Container Block";
            containerBlock.Items = new ContentArea();

            CreateSuccessStoryPage(assetFolder.ContentLink, containerBlock);

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

        private void CreateSuccessStoryPage(ContentReference containerReference, MediaCarouselContainerBlock container)
        {
            var storyPage = _contentRepository.GetDefault<SuccessStoryPage>(containerReference);

            ((IContent)storyPage).Name = "Apple";
            storyPage.Title = "Almond Apple";
            storyPage.Text = new XhtmlString("<p>Each hectare that was drip irrigated covered it's costs quicly.</p>");
            storyPage.Quote = new XhtmlString("<p>Dino Dalmasso from Morro Alto Farm</p><p>Corn, Brazil, 2016</p>");
            storyPage.Image = storyPage.CreateBlob(Thumbnail1);

            container.Items.Items.Add(new ContentAreaItem
            {
                ContentLink = _contentRepository.Save(storyPage, SaveAction.Publish, AccessLevel.NoAccess)
            });

            var storyPage2 = _contentRepository.GetDefault<SuccessStoryPage>(containerReference);
            ((IContent)storyPage2).Name = "Bananas";
            storyPage2.Title = "Bananas";
            storyPage2.Text = new XhtmlString("<p>Each hectare that was drip irrigated covered it's costs quicly.</p>");
            storyPage2.Quote = new XhtmlString("<p>Dino Dalmasso from Morro Alto Farm</p><p>Corn, Brazil, 2016</p>");
            storyPage2.Image = storyPage2.CreateBlob(Thumbnail2);

            container.Items.Items.Add(new ContentAreaItem
            {
                ContentLink = _contentRepository.Save(storyPage2, SaveAction.Publish, AccessLevel.NoAccess)
            });
        }
    }
}