using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using System.Linq;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.Quote
{
    public class QuoteGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        public QuoteGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator,
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
            var quotePage = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            quotePage.PageName = "Quote page";


            Save(quotePage);

            quotePage.Content = quotePage.Content ?? new ContentArea();

            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(quotePage.ContentLink);
            var smallQuote = _contentRepository.GetDefault<QuoteBlock>(assetsFolder.ContentLink);
            
            smallQuote = smallQuote.Init("Small Quote")
                                    .MarkAsSmall()
                                    .AddImage("110x110")
                                    .AddContent(new XhtmlString("<p>I love <strong>growing</strong> corn. After christ and my family, <strong>corn is my passion</strong></p>"))
                                    .AddAuthor("Lior Peleg, Head of Strategic Solutions at Netafim");

            quotePage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)smallQuote) });

            var bigQuote = _contentRepository.GetDefault<QuoteBlock>(assetsFolder.ContentLink);

            bigQuote = bigQuote.Init("Big Quote")
                                    .AddImage("110x110")
                                    .AddContent(new XhtmlString("<p>I love <strong>growing</strong> corn. After christ and my family, <strong>corn is my passion</strong></p>"))
                                    .AddAuthor("Lior Peleg, Head of Strategic Solutions at Netafim");

            quotePage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)bigQuote) });

            Save(quotePage);
        }

        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }

    public static class QuoteBlockBuilder
    {
        private const string DataDemoFolder = @"~/Features/Quote/Data/Demo/{0}";

        public static QuoteBlock Init(this QuoteBlock quote, string name)
        {
            quote = quote.CreateWritableClone() as QuoteBlock;
            ((IContent)quote).Name = name;

            return quote;
        }

        public static QuoteBlock MarkAsSmall(this QuoteBlock quote)
        {
            quote.IsSmallQuoteText = true;
            return quote;
        }

        public static QuoteBlock AddImage(this QuoteBlock quote, string fileName)
        {
            quote.Image = ((IContent)quote).CreateBlob(string.Format(DataDemoFolder, fileName));
            return quote;
        }

        public static QuoteBlock AddContent(this QuoteBlock quote, XhtmlString content)
        {
            quote.Content = content;
            return quote;
        }

        public static QuoteBlock AddAuthor(this QuoteBlock quote, string author)
        {
            quote.Author = author;
            return quote;
        }
    }
}