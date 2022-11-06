using EPiServer.Core;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Features.RichText
{
    public static class RichTextGeneratorDataFactory
    {
        public static RichTextWithImageAndTextBlock PopulateRichText75PercentTextBlockData(RichTextWithImageAndTextBlock block)
        {
            var generatedContent = block.CreateWritableClone() as RichTextWithImageAndTextBlock;

            generatedContent.Content = new XhtmlString(@"<p>Offering a full complement of <strong>agronomic, engineering, planning and financing services</strong>, Netafim is involved with, and accompanies customers through, all stages of the project life cycle.</p>");
            generatedContent.Title = @"Scope of services";
            generatedContent.LinkText = "OPEN SERVICES";
            generatedContent.Watermark = "Scope of services";

            ((IContent)generatedContent).Name = "Rich text 75 % text";

            return generatedContent;
        }

        public static RichTextContainerBlock PopulateRichTextContainerBlockData(RichTextContainerBlock block)
        {
            var generatedContent = block.CreateWritableClone() as RichTextContainerBlock;
            ((IContent)generatedContent).Name = "Rich text block";
            generatedContent.Watermark = "INNOVATE";
            generatedContent.IsFullWidth = false;
            generatedContent.Items = generatedContent.Items ?? new ContentArea();
            generatedContent.BackgroundColor = BackgroundColor.White;

            return generatedContent;
        }

        public static RichTextMediaBlock PopulateRichTextMediaBlockData(RichTextMediaBlock block)
        {
            var generatedContent = block.CreateWritableClone() as RichTextMediaBlock;
            ((IContent)generatedContent).Name = "Media block";
            generatedContent.Watermark = "Innovate";

            return generatedContent;
        }

        public static RichTextParagraphBlock PopulateRichTextParagraphBlockData(RichTextParagraphBlock block)
        {
            var generatedContent = block.CreateWritableClone() as RichTextParagraphBlock;
            var content = new XhtmlString(@"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
						<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem.</p>
						<h3>Boosting corn yields</h3>
						<p>At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.</p>");

            generatedContent.Content = content;
            ((IContent)generatedContent).Name = "Paragraph rich text";
            generatedContent.Title = "Subtitle";
            generatedContent.Watermark = "Innovate";

            return generatedContent;
        }

        public static RichTextTextBlock PopulateRichTextTextBlockData(RichTextTextBlock block)
        {
            var generatedContent = block.CreateWritableClone() as RichTextTextBlock;

            generatedContent.Content = new XhtmlString(@"<p>Netafim, the global leader in drip irrigation, announced today the release of a powerful new mobile app that provides corn growers who use drip irrigation with access to customized irrigation protocols and the agronomic expertise needed to boost crop productivity and reduce overall water use.</p>
					<p class="">Our <strong>new mobile app Netmaize</strong> combines <strong>real-time data</strong> to help you maximize water efficiency</p>");
            generatedContent.Title = @"Real-time data to maximize your water efficiency";
            generatedContent.LinkText = "READ MORE";
            generatedContent.Watermark = "Innovate";

            ((IContent)generatedContent).Name = "Rich text text block";

            return generatedContent;
        }

        public static RichTextWhiteBoxBlock PopulateRichTextWhiteBoxBlockData(RichTextWhiteBoxBlock block)
        {
            var generatedContent = block.CreateWritableClone() as RichTextWhiteBoxBlock;
            generatedContent.Title = "HYDROCALC";
            generatedContent.Description = "Use our user-friendly irrigation system design software to create your project.";
            generatedContent.LinkText = "DISCOVER HYDROCALC";
            ((IContent)generatedContent).Name = "Whitebox (rich text)";

            return generatedContent;
        }

        public static RichTextWhiteBoxListingBlock PopulateRichTextWhiteBoxListingBlockData(RichTextWhiteBoxListingBlock block)
        {
            var generatedContent = block.CreateWritableClone() as RichTextWhiteBoxListingBlock;
            generatedContent.Items = generatedContent.Items ?? new ContentArea();
            ((IContent)generatedContent).Name = "Whitebox listing (rich text)";
            generatedContent.Watermark = "Innovate";

            return generatedContent;
        }
    }
}