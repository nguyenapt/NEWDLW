using Castle.Core.Internal;
using Dlw.EpiBase.Content.Cms.Search;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Find;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.ProductFamily;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    [ContentGenerator(Order = 80)]
    public class ProductCategoryListingGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;
        private List<ContentReference> allCriterias = new List<ContentReference>();

        private ILogger _logger = LogManager.GetLogger();
        public ProductCategoryListingGenerator(IContentRepository contentRepository,
            IUrlSegmentCreator urlSegmentCreator,
            ContentAssetHelper contentAssetHelper
            )
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
            var homepage = _contentRepository.Get<HomePage>(context.Homepage);
            if (homepage == null) { return; }
            EnsureProductFeatureData(context);
        }

        private void EnsureProductFamilyData(ContentReference productCatalog)
        {
            var productCategoryPages = _contentRepository.GetChildren<ProductCategoryPage>(productCatalog).ToList();
            if (productCategoryPages.IsNullOrEmpty()) return;

            var familyNames = new[] { "Uniram", "DripNet PC", "Aries", "PCJ HF bubblers" };
            for (int i = 0; i < productCategoryPages.Count(); i++)
            {
                var pfamilyName = i <= familyNames.Length ? familyNames[i] : familyNames[0];
                var criterias = allCriterias.Any() ? (i <= allCriterias.Count() ? allCriterias.GetRange(0, i) : allCriterias.GetRange(0, 1)) : allCriterias; ;
                var pfamilyPage = InitPage<ProductFamilyPage>(productCategoryPages.ElementAt(i).ContentLink, pfamilyName)
                                    .AddAssocitatesToProductCategory(productCategoryPages.GetRange(0, i + 1).Select(pc => pc.ContentLink))
                                    .AddDescription()
                                    .AddThumbnail("620x620.png")
                                    .AddCriteriaToFamily(criterias);

                Save(pfamilyPage);
            }
        }

        private void EnsureProductCategoryData(ContentReference productCatalog)
        {
            var productCategoryPages = _contentRepository.GetChildren<ProductCategoryPage>(productCatalog);
            if (!productCategoryPages.IsNullOrEmpty()) return;

            var productCateNames = new[] { "Drippers and dripperlines", "DL connectors", "Sprinklers", "Filters" };
            var criteriaContainerIds = CreateCriteriaContainerData(productCatalog);
            var familyMatrixBlock = InitBlock<FamilyMatrixBlock>(_contentAssetHelper.GetOrCreateAssetFolder(productCatalog), nameof(FamilyMatrixBlock))
                                        .AddResultDescription()
                                        .AddTitle()
                                        .AddSubTitle();
            var blockContentRef = Save((IContent)familyMatrixBlock);

            foreach (var cateName in productCateNames)
            {

                var productCatePage = InitPage<ProductCategoryPage>(productCatalog, cateName)
                    .AddThumbnail("620x620.png")
                    .AddDescription()
                    .AddCriteriaToProductCategory(criteriaContainerIds);
                if (blockContentRef != null)
                {
                    productCatePage = productCatePage.AddItemsToMainContent(new List<ContentReference>() { blockContentRef });
                }
                var productCategoryRef = Save(productCatePage);

            }
        }

        private IEnumerable<ContentReference> CreateCriteriaContainerData(ContentReference productCatalog)
        {
            var criteriaContainerNames = new[] { "Segment", "Season", "Topography", "Other parameter" };
            var criteriaContainerRefs = new List<ContentReference>();
            foreach (var name in criteriaContainerNames)
            {
                var criteriaContainerPage = InitPageNoTemplate<CriteriaContainerPage>(productCatalog, name);
                var criteriaContainerRef = Save(criteriaContainerPage);
                var criteriaNames = GetCriteriaNames(name);


                foreach (var criteriaName in criteriaNames)
                {
                    ContentReference criteriaRef = ContentReference.EmptyReference;
                    CriteriaPage criteriaPage;
                  
                    criteriaPage = InitPageNoTemplate<CriteriaPage>(criteriaContainerRef, criteriaName);
                  
                    criteriaRef = Save(criteriaPage);
                    if (!ContentReference.IsNullOrEmpty(criteriaRef)) { allCriterias.Add(criteriaRef); }
                }

                criteriaContainerRefs.Add(criteriaContainerRef);
            }

            return criteriaContainerRefs;
        }

        private string[] GetCriteriaNames(string containerName)
        {
            if (containerName.Equals("Segment")) { return new[] { "Open fields", "Orchards" }; }
            if (containerName.Equals("Season")) { return new[] { "1-3 seasons", "3-8 season" }; }
            if (containerName.Equals("Topography")) { return new[] { "Flat topography", "Uneven topography" }; }
            if (containerName.Equals("Other parameter")) { return new[] { "For pulse irrigation" }; }
            return new string[] { };
        }

        private void EnsureCategoryListingData(ContentReference productCatalogRef)
        {
            var productCatalog = _contentRepository.Get<ProductCatalogPage>(productCatalogRef);
            if (productCatalog.Content != null && productCatalog.Content.Items != null && productCatalog.Content.Items.Any(x => x.GetContent() is ProductCategoryListingBlock))
            {
                return;
            }
            var productListingBlock = InitBlock<ProductCategoryListingBlock>(_contentAssetHelper.GetOrCreateAssetFolder(productCatalogRef),
                nameof(ProductCategoryListingBlock));
            var blockContentRef = Save((IContent)productListingBlock);

            productCatalog = productCatalog.AddItemsToMainContent(new[] { blockContentRef });
            Save(productCatalog);
        }

        private void EnsureProductFeatureData(ContentContext context)
        {
            var productCatalogs = _contentRepository.GetChildren<ProductCatalogPage>(context.Homepage);
            if (!productCatalogs.IsNullOrEmpty()) return;
            var productCatalogPage = InitPage<ProductCatalogPage>(context.Homepage, "Products Catalog");
            var productCatalogRef = Save(productCatalogPage);

            if (productCatalogPage == null) { return; }

            EnsureCategoryListingData(productCatalogRef);

            EnsureProductCategoryData(productCatalogRef);

            EnsureProductFamilyData(productCatalogRef);
        }

        private T InitBlock<T>(ContentAssetFolder folder, string name) where T : BlockData
        {
            var block = _contentRepository.GetDefault<T>(folder.ContentLink);
            ((IContent)block).Name = name;
            return block;
        }

        private T InitPage<T>(ContentReference parent, string title) where T : GenericContainerPage
        {
            var page = _contentRepository.GetDefault<T>(parent);
            page.PageName = title;
            page.Title = title;
            page.URLSegment = _urlSegmentCreator.Create(page);
            return page;
        }

        private T InitPageNoTemplate<T>(ContentReference parent, string title) where T : NoTemplatePageBase
        {
            var page = _contentRepository.GetDefault<T>(parent);
            page.PageName = title;
            page.URLSegment = _urlSegmentCreator.Create(page);
            return page;
        }
        private ContentReference Save<T>(T page) where T : IContent
        {
            return _contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess);
        }


    }
}