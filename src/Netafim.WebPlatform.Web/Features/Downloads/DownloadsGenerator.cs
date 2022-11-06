using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Features.Home;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.Downloads
{
    [ContentGenerator(Order = 20)]
    public class DownloadsGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public DownloadsGenerator(IContentRepository contentRepo, ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepo;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var homepage = _contentRepository.Get<HomePage>(context.Homepage).CreateWritableClone() as HomePage;

            if (homepage.Content != null && homepage.Content.FilteredItems.Any(IsDownloadsContainer))
            {
                return;
            }

            var homepageAssetFolder = _contentAssetHelper.GetOrCreateAssetFolder(context.Homepage);

            var downloadBlock = GenerateBlock<DownloadsBlock>(homepageAssetFolder, "PRODUCT  SHEETS");
            var downloadContainer = GenerateBlock<DownloadsContainerBlock>(homepageAssetFolder, "Downloads");
            var downloadContainerBlockRef = UpdateDownloadsContainer(downloadContainer, downloadBlock);

            if (ContentReference.IsNullOrEmpty(downloadContainerBlockRef)) { return; }

            if (homepage.Content == null)
            {
                homepage.Content = new ContentArea();
            }

            homepage.Content.Items.Insert(homepage.Content.Items.Count, new ContentAreaItem()
            {
                ContentLink = downloadContainerBlockRef
            });

            _contentRepository.Save(homepage, SaveAction.Publish, AccessLevel.NoAccess);

            context.Homepage = homepage.ContentLink;
        }

        private ContentReference UpdateDownloadsContainer(ContentReference downloadContainer, ContentReference downloadBlock)
        {
            if (ContentReference.IsNullOrEmpty(downloadBlock) || ContentReference.IsNullOrEmpty(downloadContainer)) return ContentReference.EmptyReference;

            DownloadsContainerBlock containerBlock;
            if (!_contentRepository.TryGet(downloadContainer, out containerBlock)) { return ContentReference.EmptyReference; }

            var clone = containerBlock.CreateWritableClone() as DownloadsContainerBlock;
            clone.Items = new ContentArea();
            var totalItems = 3;
            for (int i = 0; i < totalItems; i++)
            {
                clone.Items.Items.Add(new ContentAreaItem()
                {
                    ContentLink = downloadBlock
                });
            }

            return _contentRepository.Save((IContent)clone, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private ContentReference GenerateBlock<T>(ContentAssetFolder homepageAssetFolder, string blockName) where T : BlockData
        {
            var block = _contentRepository.GetDefault<T>(homepageAssetFolder.ContentLink);
            ((IContent)block).Name = blockName;
            if (block.Property.Contains("Title"))
            {
                ((IContent)block).Property["Title"].Value = blockName;
            }
            if (block.Property.Contains("Watermark"))
            {
                ((IContent)block).Property["Watermark"].Value = blockName;
            }
            return _contentRepository.Save((IContent)block, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private bool IsDownloadsContainer(ContentAreaItem arg)
        {
            var content = _contentRepository.Get<IContent>(arg.ContentLink);

            return content is DownloadsContainerBlock;
        }
    }
}