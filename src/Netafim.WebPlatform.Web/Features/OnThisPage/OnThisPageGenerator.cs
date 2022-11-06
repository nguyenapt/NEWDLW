using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Features.Home;

namespace Netafim.WebPlatform.Web.Features.OnThisPage
{
    public class OnThisPageGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public OnThisPageGenerator(
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

            if (homepage?.Content != null && homepage.Content.FilteredItems.Any(IsOnthispageComponent))
            {
                return;
            }

            var onThisPageBlockRef = EnsureBlock(assetFolder.ContentLink);

            homepage.Content = homepage.Content ?? new ContentArea();
            
            homepage.Content.Items.Add(new ContentAreaItem
            {
                ContentLink = onThisPageBlockRef
            });

            _contentRepository.Save(homepage, SaveAction.Publish, AccessLevel.NoAccess);

            context.Homepage = homepage.ContentLink;
        }

        private ContentReference EnsureBlock(ContentReference onThisPageRef)
        {
            var block = _contentRepository.GetDefault<OnThisPageBlock>(onThisPageRef);
            ((IContent)block).Name = "onThisPageBlock";
            block.Title = "Other product categories";
            block.SubTitle = "Other Dripperlines";
            block.Watermark = "Dripperlines";

            return _contentRepository.Save((IContent)block, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private bool IsOnthispageComponent(ContentAreaItem arg)
        {
            var content = _contentRepository.Get<IContent>(arg.ContentLink);

            return content is OnThisPageBlock;
        }
    }
}