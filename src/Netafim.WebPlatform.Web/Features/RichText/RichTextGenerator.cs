using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using System;
using System.Linq;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.RichText
{
    public class RichTextGenerator : IContentGenerator
    {
        private const string DataDemoFolder = @"~/Features/RichText/Data/Demo/{0}";

        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        public RichTextGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator,
            ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _urlSegmentCreator = urlSegmentCreator;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var richTextContainerPage = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            richTextContainerPage.PageName = "Rich text page";

            if (richTextContainerPage.Content != null && richTextContainerPage.Content.FilteredItems.Any(IsRichTextBlock))
            {
                return;
            }

            Save(richTextContainerPage);

            richTextContainerPage.Content = richTextContainerPage.Content ?? new ContentArea();

            // Generate the rich text component and add to the richtext container

            richTextContainerPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = GenerateRichTextContainer(richTextContainerPage.ContentLink, GenerateTextWith75PercentWidthWithImage)
            });

            richTextContainerPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = GenerateRichTextContainer(richTextContainerPage.ContentLink, GenerateDefaultTextBlock, GenerateWhiteBox)
            });

            richTextContainerPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = GenerateRichTextContainer(richTextContainerPage.ContentLink, GenerateTextWith75PercentWidthWithReadmore)
            });

            richTextContainerPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = GenerateRichTextContainer(richTextContainerPage.ContentLink, GenerateParagraphBlock)
            });

            richTextContainerPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = GenerateRichTextContainer(richTextContainerPage.ContentLink, GenerateDefaultTextBlock, GenerateImageBlock)
            });

            richTextContainerPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = GenerateRichTextContainer(richTextContainerPage.ContentLink, GenerateImageBlock, GenerateDefaultTextBlock)
            });

            richTextContainerPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = GenerateRichTextContainer(richTextContainerPage.ContentLink, GenerateTextWithBullet, GenerateVideoBlock)
            });

            Save(richTextContainerPage);            
        }

        private ContentReference GenerateRichTextContainer(ContentReference contentReference, params Action<ContentAssetFolder, RichTextContainerBlock>[] generateContents)
        {
            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(contentReference);
            var richTextContainer = _contentRepository.GetDefault<RichTextContainerBlock>(assetsFolder.ContentLink);
            richTextContainer = RichTextGeneratorDataFactory.PopulateRichTextContainerBlockData(richTextContainer);

            // Generate paragraph block
            generateContents.ToList().ForEach(generator => generator(assetsFolder, richTextContainer));
            
            return _contentRepository.Save((IContent)richTextContainer, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private void GenerateParagraphBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            var richTextParagraphBlock = _contentRepository.GetDefault<RichTextParagraphBlock>(contentAssetFolder.ContentLink);
            richTextParagraphBlock = RichTextGeneratorDataFactory.PopulateRichTextParagraphBlockData(richTextParagraphBlock);

            var paragraph = _contentRepository.Save((IContent)richTextParagraphBlock, SaveAction.Publish, AccessLevel.NoAccess);
            
            container.Items.Items.Add(new ContentAreaItem()
            {
                ContentLink = paragraph
            });
        }

        private void GenerateVideoBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            GenerateMediaBlock(contentAssetFolder, container, true);
        }

        private void GenerateImageBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            GenerateMediaBlock(contentAssetFolder, container, false);
        }

        private void GenerateDefaultTextBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            GenerateTextBlock(contentAssetFolder, container, false);
        }

        private void GenerateTextWithBullet(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            GenerateTextBlock(contentAssetFolder, container, true);
        }

        private void GenerateTextBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container, bool textWithBullets)
        {
            var richTextTextBlock = _contentRepository.GetDefault<RichTextTextBlock>(contentAssetFolder.ContentLink);
            richTextTextBlock = RichTextGeneratorDataFactory.PopulateRichTextTextBlockData(richTextTextBlock);
            if(textWithBullets)
            {
                var iconUrl = "http://via.placeholder.com/40x40";
                richTextTextBlock.Content = new XhtmlString($@"<ul class='list-with-icon'>
						<li>
							<img alt='' src='{iconUrl}'>
							<p>Smart irrigation saves up to <strong>50% of water</strong> compared to water channels &amp; flood</p>
						</li>
						<li>
							<img alt='' src='{iconUrl}'>
							<p>Fertigation via drip is over <strong>30% more</strong> efficient than via flooding</p>
						</li>
						<li>
							<img alt='' src='{iconUrl}'>
							<p>The combination of drip irrigation &amp; fertigation increases productivity by <strong>up to 200%</strong></p>
						</li>
					</ul>");
            }
            richTextTextBlock.Link = ContentReference.StartPage;
            container.IsFullWidth = true;

            var richTextReference = _contentRepository.Save((IContent)richTextTextBlock, SaveAction.Publish, AccessLevel.NoAccess);

            container.Items.Items.Add(new ContentAreaItem() { ContentLink = richTextReference });
        }
        
        private void GenerateWhiteBox(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            // Reset the full mode
            container.IsFullWidth = false;
            container.BackgroundColor = Infrastructure.Epi.Editors.BackgroundColor.Grey;

            var whiteBoxListing = _contentRepository.GetDefault<RichTextWhiteBoxListingBlock>(contentAssetFolder.ContentLink);
            whiteBoxListing = RichTextGeneratorDataFactory.PopulateRichTextWhiteBoxListingBlockData(whiteBoxListing);

            // Add whitebox item
            GenerateWhiteBoxItem(contentAssetFolder, whiteBoxListing);
            GenerateWhiteBoxItem(contentAssetFolder, whiteBoxListing);

            var whiteBoxListingReference = _contentRepository.Save((IContent)whiteBoxListing, SaveAction.Publish, AccessLevel.NoAccess);

            container.Items.Items.Add(new ContentAreaItem() { ContentLink = whiteBoxListingReference });
        }

        private void GenerateWhiteBoxItem(ContentAssetFolder contentAssetFolder, RichTextWhiteBoxListingBlock whiteBoxListing)
        {
            var whiteBox = _contentRepository.GetDefault<RichTextWhiteBoxBlock>(contentAssetFolder.ContentLink);
            whiteBox = RichTextGeneratorDataFactory.PopulateRichTextWhiteBoxBlockData(whiteBox);
            whiteBox.Link = ContentReference.StartPage;
            whiteBox.Image = ((IContent)whiteBox).CreateBlob(MediaPath("54x54.png"));

            var whiteBoxReference = _contentRepository.Save((IContent)whiteBox, SaveAction.Publish, AccessLevel.NoAccess);

            whiteBoxListing.Items.Items.Add(new ContentAreaItem() { ContentLink = whiteBoxReference });
        }

        private void GenerateTextWith75PercentWidthWithImage(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            GenerateTextWith75PercentWidth(contentAssetFolder, container, true);
        }

        private void GenerateTextWith75PercentWidthWithReadmore(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            GenerateTextWith75PercentWidth(contentAssetFolder, container, false);
        }

        private void GenerateTextWith75PercentWidth(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container, bool hasMedia)
        {
            var textWith75Percent = _contentRepository.GetDefault<RichTextWithImageAndTextBlock>(contentAssetFolder.ContentLink);
            textWith75Percent = RichTextGeneratorDataFactory.PopulateRichText75PercentTextBlockData(textWith75Percent);

            if(hasMedia)
            {
                textWith75Percent.Image = ((IContent)textWith75Percent).CreateBlob(MediaPath("farm2.jpg"));
            }
            else
            {
                textWith75Percent.LinkText = "OPEN SERVICES";
                textWith75Percent.Link = ContentReference.StartPage;
            }

            var textWith75PercentReference = _contentRepository.Save((IContent)textWith75Percent, SaveAction.Publish, AccessLevel.NoAccess);

            container.Items.Items.Add(new ContentAreaItem() { ContentLink = textWith75PercentReference });
        }

        private void GenerateMediaBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container, bool isVideo)
        {
            var imageMedia = isVideo ? "testimonial_senegal.png" : "farm1.jpg";
            var videoMedia = "samplevideo.mp4";

            var mediaBlock = _contentRepository.GetDefault<RichTextMediaBlock>(contentAssetFolder.ContentLink);
            mediaBlock = RichTextGeneratorDataFactory.PopulateRichTextMediaBlockData(mediaBlock);
            mediaBlock.Image = ((IContent)mediaBlock).CreateBlob(MediaPath(imageMedia));
            if(isVideo)
            {
                mediaBlock.Video = ((IContent)mediaBlock).CreateBlob(MediaPath(videoMedia));
            }

            var mediaBlockReference = _contentRepository.Save((IContent)mediaBlock, SaveAction.Publish, AccessLevel.NoAccess);

            container.Items.Items.Add(new ContentAreaItem() { ContentLink = mediaBlockReference });
        }

        private string MediaPath(string fileName) => string.Format(DataDemoFolder, fileName);

        private bool IsRichTextBlock(ContentAreaItem arg)
        {
            var content = _contentRepository.Get<IContent>(arg.ContentLink);

            return content is RichTextContainerBlock;
        }
        
        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}