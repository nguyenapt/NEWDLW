using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Features.Accordion
{
    public class AccordionContentGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public AccordionContentGenerator(IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
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

            if (homepage?.Content != null && homepage.Content.FilteredItems.Any(IsMediaCarouselComponent))
            {
                return;
            }

            var containerBlock = _contentRepository.GetDefault<AccordionContainerBlock>(assetFolder.ContentLink);
            ((IContent)containerBlock).Name = "FAQs Container Block";
            containerBlock.Title = "FAQs (Myths and facts)";
            containerBlock.Watermark = "Myths and facts";
            containerBlock.VerticalText = "Myths and facts";
            containerBlock.BackgroundColor = BackgroundColor.Grey;
            containerBlock.Items = new ContentArea();
            GenerateFaqItems(containerBlock, assetFolder.ContentLink);

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

        private void GenerateFaqItems(AccordionContainerBlock containerBlock, ContentReference containerReference)
        {
            var faqItem1 = _contentRepository.GetDefault<FAQItemBlock>(containerReference);
            ((IContent)faqItem1).Name = "FAQ item 1";
            faqItem1.Question = new XhtmlString("Irrigation systems aren't needed in areas with good amounts of rainfall");
            faqItem1.Answer = new XhtmlString("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.");

            containerBlock.Items.Items.Add(new ContentAreaItem
            {
                ContentLink = _contentRepository.Save((IContent) faqItem1, SaveAction.Publish, AccessLevel.NoAccess)
            });

            var faqItem2 = _contentRepository.GetDefault<FAQItemBlock>(containerReference);
            ((IContent)faqItem2).Name = "FAQ item 1";
            faqItem2.Question = new XhtmlString("1914 translation by H. Rackham");
            faqItem2.Answer = new XhtmlString("On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain");

            containerBlock.Items.Items.Add(new ContentAreaItem
            {
                ContentLink = _contentRepository.Save((IContent)faqItem2, SaveAction.Publish, AccessLevel.NoAccess)
            });
        }

        private bool IsMediaCarouselComponent(ContentAreaItem arg)
        {
            var content = _contentRepository.Get<IContent>(arg.ContentLink);

            return content is AccordionContainerBlock;
        }
    }
}