using System;
using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.JobApplicationForm;
using Netafim.WebPlatform.Web.Features.RichText;
using Netafim.WebPlatform.Web.Features.RichText.Models;

namespace Netafim.WebPlatform.Web.Features.CustomForm
{
    public class CustomFormContentGenerator : IContentGenerator
    {
        private const string Icon = "~/Features/CustomForm/Data/Demo/50x70.png";

        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public CustomFormContentGenerator(
            IContentRepository contentRepository,
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
            var assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(context.Homepage);
            if (homepage?.Content != null && homepage.Content.FilteredItems.Any(IsCustomFormComponent))
            {
                return;
            }

            var richTextBlockRef = GenerateRichTextContainer(assetFolder, GenerateTextWithBullet, CustomFormBlock);

            homepage.Content = homepage.Content ?? new ContentArea();
            homepage.Content.Items.Add(new ContentAreaItem
            {
                ContentLink = richTextBlockRef
            });

            Save(homepage);

            context.Homepage = homepage.ContentLink;
        }

        private ContentReference GenerateRichTextContainer(ContentAssetFolder contentAssetFolder, params Action<ContentAssetFolder, RichTextContainerBlock>[] generateContents)
        {
            var richTextContainer = _contentRepository.GetDefault<RichTextContainerBlock>(contentAssetFolder.ContentLink);
            richTextContainer = RichTextGeneratorDataFactory.PopulateRichTextContainerBlockData(richTextContainer);

            // Generate paragraph block
            generateContents.ToList().ForEach(generator => generator(contentAssetFolder, richTextContainer));

            return Save((IContent)richTextContainer);
        }

        private void CustomFormBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            var block = _contentRepository.GetDefault<CustomFormContainerBlock>(contentAssetFolder.ContentLink);
            ((IContent)block).Name = "customFormBlock";
            block.Title = "MORE INFO?";
            block.Description = "Contact us and one of our experts will get back to you shortly.";
            block.FormIcon = ((IContent)block).CreateBlob(Icon);

            var folders = _contentRepository.GetDefault<ContentFolder>(ContentReference.SiteBlockFolder);
            var formFolder = _contentRepository.GetChildren<ContentFolder>(folders.ParentLink)
                                                .SingleOrDefault(t => t.Name == "Episerver Forms");

            block.FormContainer = block.FormContainer ?? new ContentArea();
            block.FormContainer.Items.Add(new ContentAreaItem()
            {
                ContentLink = GenerateEpiServerForm(formFolder)
            });

            var blockRef = Save((IContent)block);

            container.Items.Items.Add(new ContentAreaItem() { ContentLink = blockRef });
        }

        private void GenerateTextWithBullet(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            GenerateTextBlock(contentAssetFolder, container, true);
        }

        private void GenerateTextBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container, bool textWithBullets)
        {
            var richTextTextBlock = _contentRepository.GetDefault<RichTextTextBlock>(contentAssetFolder.ContentLink);
            richTextTextBlock = RichTextGeneratorDataFactory.PopulateRichTextTextBlockData(richTextTextBlock);
            if (textWithBullets)
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
            container.IsFullWidth = false;

            var richTextReference = Save((IContent)richTextTextBlock);

            container.Items.Items.Add(new ContentAreaItem() { ContentLink = richTextReference });
        }

        private bool IsCustomFormComponent(ContentAreaItem arg)
        {
            var content = _contentRepository.Get<IContent>(arg.ContentLink);

            return content is CustomFormContainerBlock;
        }

        private ContentReference GenerateEpiServerForm(ContentFolder formFolder)
        {
            var container = EpiFormGeneratorDataFactory.CreateFormContainerBlock(formFolder, "Custom form");

            // full name
            var tbFullname = EpiFormGeneratorDataFactory.CreateTextboxElementBlock(formFolder, "fullname", string.Empty, "Full name");
            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)tbFullname)
            });

            // email address
            var tbEmail = EpiFormGeneratorDataFactory.CreateTextboxElementBlock(formFolder, "email", string.Empty, "E-mail address");
            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)tbEmail)
            });

            // submit button
            var submitButton = EpiFormGeneratorDataFactory.CreateSubmitButtonElementBlock(formFolder, "request", "request info");
            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)submitButton)
            });

            return Save((IContent)container);
        }

        private ContentReference Save(IContent content)
        {
            return _contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}