using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.Home;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    [ContentGenerator(Order = 50)]
    public class MediaCarouselContentGenerator : IContentGenerator
    {
        private const string Thumbnail1 = "~/Features/MediaCarousel/Data/Demo/farm1.jpg";
        private const string Thumbnail2 = "~/Features/MediaCarousel/Data/Demo/testimonial_senegal.png";
        private const string Video1 = "~/Features/MediaCarousel/Data/Demo/niteco.mp4";

        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public MediaCarouselContentGenerator(IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
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

            var imageCarouselBlock = CreateImageCarouselBlock(assetFolder.ContentLink);
            var videoCarouselBlock = CreateVideoCarouselBlock(assetFolder.ContentLink);

            var containerBlock = _contentRepository.GetDefault<MediaCarouselContainerBlock>(assetFolder.ContentLink);
            ((IContent)containerBlock).Name = "Carousel Container Block";
            containerBlock.Items = new ContentArea();
            containerBlock.Items.Items.Add(new ContentAreaItem
            {
                ContentLink = imageCarouselBlock
            });
            containerBlock.Items.Items.Add(new ContentAreaItem
            {
                ContentLink = videoCarouselBlock
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

        ContentReference CreateImageCarouselBlock(ContentReference containerReference)
        {
            var imageCarouselBlock = _contentRepository.GetDefault<ImageCarouselBlock>(containerReference);

            ((IContent)imageCarouselBlock).Name = "Image carousel block";
            imageCarouselBlock.Title = "Image carousel block";
            imageCarouselBlock.Text = new XhtmlString("<p>Since we started using drip we've increased production by up to 35-40%</p>");
            imageCarouselBlock.Link = new Url("https://www.google.com");
            imageCarouselBlock.Quote = new XhtmlString("<strong>Dino Dalmasso from Morro Alto Farm</strong><p>Corn, Brazil, 2016</>p");

            imageCarouselBlock.Image = ((IContent)imageCarouselBlock).CreateBlob(Thumbnail1);

            return _contentRepository.Save((IContent)imageCarouselBlock, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private ContentReference CreateVideoCarouselBlock(ContentReference containerReference)
        {
            var videoCarouselBlock = _contentRepository.GetDefault<VideoCarouselBlock>(containerReference);

            ((IContent)videoCarouselBlock).Name = "Video carousel block";
            videoCarouselBlock.Title = "Video carousel block";
            videoCarouselBlock.Text = new XhtmlString("<p>Each hectare that was drip irrigated covered it's costs quicly.</p>");
            videoCarouselBlock.Link = new Url("https://www.google.com");
            videoCarouselBlock.Quote = new XhtmlString("<p>Dino Dalmasso from Morro Alto Farm</p><p>Corn, Brazil, 2016</p>");
            videoCarouselBlock.Image = ((IContent) videoCarouselBlock).CreateBlob(Thumbnail2);
            videoCarouselBlock.Video = ((IContent) videoCarouselBlock).CreateBlob(Video1);

            return _contentRepository.Save((IContent)videoCarouselBlock, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}