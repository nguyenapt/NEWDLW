using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using System;
using EPiServer.DataAccess;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Features.CropsOverview;
using Netafim.WebPlatform.Web.Features.GenericOverview;
using Netafim.WebPlatform.Web.Features.MediaCarousel;
using Netafim.WebPlatform.Web.Features.PageTitle;
using Netafim.WebPlatform.Web.Features.RichText.Models;

namespace Netafim.WebPlatform.Web.Infrastructure.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class TemplateInitialization : IInitializableModule
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(TemplateInitialization));

        private IContentRepository _contentRepository;
        private IUrlSegmentGenerator _urlSegementGenerator;
        private ContentAssetHelper _contentAssetHelper;

        public void Initialize(InitializationEngine context)
        {
            _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            _urlSegementGenerator = ServiceLocator.Current.GetInstance<IUrlSegmentGenerator>();
            _contentAssetHelper = ServiceLocator.Current.GetInstance<ContentAssetHelper>();

            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.CreatedContent += ContentEvents_CreatedContent;
        }

        private void ContentEvents_CreatedContent(object sender, ContentEventArgs e)
        {
            if (e.Content is CropsPage)
            {
                CreateDefaultComponentsCropsPage((CropsPage) e.Content);
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.CreatedContent -= ContentEvents_CreatedContent;
        }

        private void CreateDefaultComponentsCropsPage(CropsPage content)
        {
            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(content.ContentLink);

            var page = content.CreateWritableClone() as CropsPage;

            if (page.Content == null)
            {
                page.Content = new ContentArea();
            }

            CreatePageTitle(page, assetsFolder);
            CreateRichText(page, assetsFolder);
            CreateCarousel(page, assetsFolder);
            CreateRichTextTwoColumns(page, assetsFolder);
            CreateRichTextTwoColumns(page, assetsFolder);
            CreateCarousel(page, assetsFolder);
            CreateRichText(page, assetsFolder);
            CreateCarousel(page, assetsFolder);
            CreateGenericOverview(page, assetsFolder);

            _contentRepository.Save(page);
        }

        private void CreateCarousel(CropsPage page, ContentAssetFolder assetsFolder)
        {
            var imageCarousel = _contentRepository.GetDefault<MediaCarouselContainerBlock>(assetsFolder.ContentLink) as IContent;
            imageCarousel.Name = $"{page.Name} - MediaCarousel";
            _contentRepository.Save(imageCarousel, SaveAction.Publish, AccessLevel.NoAccess);

            page.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = imageCarousel.ContentLink
            });
        }

        private void CreatePageTitle(CropsPage page, ContentAssetFolder assetsFolder)
        {
            var pageTitle = _contentRepository.GetDefault<PageTitleBlock>(assetsFolder.ContentLink) as IContent;
            pageTitle.Name = $"{page.Name} - PageTitle";
            _contentRepository.Save(pageTitle, SaveAction.Publish, AccessLevel.NoAccess);

            page.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = pageTitle.ContentLink
            });
        }

        private void CreateRichText(CropsPage page, ContentAssetFolder assetsFolder)
        {
            var richText = _contentRepository.GetDefault<RichTextContainerBlock>(assetsFolder.ContentLink) as IContent;
            richText.Name = $"{page.Name} - RichText";
            _contentRepository.Save(richText, SaveAction.Publish, AccessLevel.NoAccess);

            page.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = richText.ContentLink
            });
        }

        private void CreateRichTextTwoColumns(CropsPage page, ContentAssetFolder assetsFolder)
        {
            var richText = _contentRepository.GetDefault<RichTextContainerBlock>(assetsFolder.ContentLink) as IContent;
            richText.Name = $"{page.Name} - RichText";
            _contentRepository.Save(richText, SaveAction.Publish, AccessLevel.NoAccess);

            var textBlock = _contentRepository.GetDefault<RichTextTextBlock>(assetsFolder.ContentLink);
            ((IContent)textBlock).Name = "TextBlock";
            var textReference = _contentRepository.Save((IContent)textBlock, SaveAction.Publish, AccessLevel.NoAccess);

            var mediaBlock = _contentRepository.GetDefault<RichTextMediaBlock>(assetsFolder.ContentLink);
            ((IContent)mediaBlock).Name = "MediaBlock";
            var mediaBlockReference = _contentRepository.Save((IContent)mediaBlock, SaveAction.Publish, AccessLevel.NoAccess);

            ((RichTextContainerBlock)richText).Items = new ContentArea();
            ((RichTextContainerBlock)richText).Items.Items.Add(new ContentAreaItem() { ContentLink = textReference });
            ((RichTextContainerBlock)richText).Items.Items.Add(new ContentAreaItem() { ContentLink = mediaBlockReference });

            page.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = richText.ContentLink
            });
        }

        private void CreateGenericOverview(CropsPage page, ContentAssetFolder assetsFolder)
        {
            var genericOverview = _contentRepository.GetDefault<GenericOverviewBlock>(assetsFolder.ContentLink) as IContent;
            genericOverview.Name = $"{page.Name} - GenericOverview";
            _contentRepository.Save(genericOverview, SaveAction.Publish, AccessLevel.NoAccess);

            page.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = genericOverview.ContentLink
            });
        }

    }
}