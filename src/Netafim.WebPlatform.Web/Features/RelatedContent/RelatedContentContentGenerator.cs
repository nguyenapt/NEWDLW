using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Home;

namespace Netafim.WebPlatform.Web.Features.RelatedContent
{
    public class RelatedContentContentGenerator : IContentGenerator
    {
        private const string Thumbnail1 = "~/Features/RelatedContent/Data/Demo/farm1.jpg";
        private const string Thumbnail2 = "~/Features/RelatedContent/Data/Demo/farm2.jpg";

        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        public RelatedContentContentGenerator(IContentRepository contentRepository,
            IUrlSegmentCreator urlSegmentCreator, ContentAssetHelper contentAssetHelper)
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
            var homepage = _contentRepository.Get<HomePage>(context.Homepage).CreateWritableClone() as HomePage;
            
            if (homepage.Content != null && homepage.Content.FilteredItems.Any(IsRelatedContentComponent))
            {
                return;
            }

            var homepageAssetFolder = _contentAssetHelper.GetOrCreateAssetFolder(context.Homepage);

            var data = EnsureData(context);

            var block = _contentRepository.GetDefault<RelatedContentBlock>(homepageAssetFolder.ContentLink);
            ((IContent)block).Name = "relatedContentBlock";
            block.Items = new ContentArea();
            block.Items.Items.Add(new ContentAreaItem()
            {
                ContentLink = data
            });

            var blockReference = _contentRepository.Save((IContent) block, SaveAction.Publish, AccessLevel.NoAccess);

            if (homepage.Content == null)
            {
                homepage.Content = new ContentArea();
            }

            homepage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = blockReference
            });

            _contentRepository.Save(homepage, SaveAction.Publish, AccessLevel.NoAccess);

            context.Homepage = homepage.ContentLink;
        }

        private ContentReference EnsureData(ContentContext context)
        {
            var relatedContentPage = _contentRepository.GetDefault<GenericContainerPage>(context.Homepage);

            relatedContentPage.PageName = "Related content page";
            relatedContentPage.URLSegment = _urlSegmentCreator.Create(relatedContentPage);
            relatedContentPage.Title = "Related content page";
            relatedContentPage.Description = new XhtmlString(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae pretium neque. Nam lorem dui, convallis in risus bibendum, vehicula eleifend sapien. Ut id lectus tristique, fringilla mauris in, porta nisl. Cras sed libero sit amet metus varius euismod in in ligula. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Proin quis nibh lectus.");

            relatedContentPage.Thumnbail = relatedContentPage.CreateBlob(Thumbnail1);

            return _contentRepository.Save(relatedContentPage, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private bool IsRelatedContentComponent(ContentAreaItem arg)
        {
            var content = _contentRepository.Get<IContent>(arg.ContentLink);

            return content is RelatedContentBlock;
        }
    }
}