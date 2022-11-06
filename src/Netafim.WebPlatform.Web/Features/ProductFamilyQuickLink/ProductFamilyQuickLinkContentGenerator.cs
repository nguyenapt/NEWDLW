using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Features.ProductCategory;

namespace Netafim.WebPlatform.Web.Features.ProductFamilyQuickLink
{
    [ContentGenerator(Order = 90)]
    public class ProductFamilyQuickLinkContentGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public ProductFamilyQuickLinkContentGenerator(IContentRepository contentRepository,
            ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            GenerateComponent(context);
        }

        private void GenerateComponent(ContentContext context)
        {
            var catalog = _contentRepository.GetChildren<ProductCatalogPage>(context.Homepage).SingleOrDefault();
            if(catalog == null)
                return;

            var categories = _contentRepository.GetChildren<ProductCategoryPage>(catalog.ContentLink);
            if(categories == null || !categories.Any())
                return;

            var assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(context.Homepage);
            var blockRef = CreateProductFamilyQuickLinkBlock(assetFolder.ContentLink);
            var categoryWritable = categories.Select(t => _contentRepository.Get<ProductCategoryPage>(t.ContentLink).CreateWritableClone() as ProductCategoryPage);

            foreach (var cate in categoryWritable)
            {
                cate.Content = cate.Content ?? new ContentArea();
                cate.Content.Items.Add(new ContentAreaItem()
                {
                    ContentLink = blockRef
                });

                Save(cate);
            }
        }

        private ContentReference CreateProductFamilyQuickLinkBlock(ContentReference onThisPageRef)
        {
            var block = _contentRepository.GetDefault<ProductFamilyQuickLinkBlock>(onThisPageRef);
            ((IContent)block).Name = "productFamilyQuickLinkBlock";
            block.Title = "Quick links";

            return Save((IContent)block);
        }

        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}