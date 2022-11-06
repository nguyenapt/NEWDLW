using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    public class GenericOverviewGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public GenericOverviewGenerator(
            IContentRepository contentRepository, 
            ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            GenerateData(context);
        }

        private void GenerateData(ContentContext context)
        {
            var ctaContainerPage = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            ctaContainerPage.PageName = "Overview Page";
            var ctaRefenrece = Save(ctaContainerPage);
            ctaContainerPage.Content = ctaContainerPage.Content ?? new ContentArea();

            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(ctaRefenrece);

            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = GenerateOverview(context, assetsFolder, true) });
            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = GenerateOverview(context, assetsFolder) });
            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = GenerateOverviewWithThumnail(context, assetsFolder, 1) });
            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = GenerateOverviewWithThumnail(context, assetsFolder, 2) });
            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = GenerateSustainableOverview(context, assetsFolder) });

            Save(ctaContainerPage);
        }

        private ContentReference GenerateOverviewWithThumnail(ContentContext context, ContentAssetFolder assetsFolder, int totalRows = 1)
        {
            var overview = _contentRepository.GetDefault<GenericOverviewBlock>(assetsFolder.ContentLink);
            overview = overview.Init();
            int columnCount = 3;
            Save((IContent)overview);

            for (int i = 0; i < totalRows * columnCount; i++)
            {
                var item = _contentRepository.GetDefault<GenericOverviewItemWithThumbnailBlock>(assetsFolder.ContentLink);
                item = item.Init("PLANET & ENVIRONMENT")
                    .AddLink(context.Homepage)
                    .AddThumbnail("310x250.png");
                item.LinkText = "READ MORE";
                overview.AddItem(Save((IContent)item));
            }

            return Save((IContent)overview);
        }

        private ContentReference GenerateSustainableOverview(ContentContext context, ContentAssetFolder contentAssetFolder)
        {
            var overview = this._contentRepository.GetDefault<GenericOverviewBlock>(contentAssetFolder.ContentLink);
            overview = overview.Init();
            overview.BlueLink = true;
            Save((IContent)overview);

            for (int i = 0; i < 5; i++)
            {
                var item = this._contentRepository.GetDefault<SustainableItemBlock>(contentAssetFolder.ContentLink);
                item = item.Init("FINANCE")
                          .AddIcon("620x620.png")
                          .AddLink(context.Homepage);
                
                overview.AddItem(Save((IContent)item));
            }

            return Save((IContent)overview);
        }
        
        private ContentReference GenerateOverview(ContentContext context, ContentAssetFolder assetsFolder, bool itemHasDescription = false)
        {
            var overview = this._contentRepository.GetDefault<GenericOverviewBlock>(assetsFolder.ContentLink);
            overview = overview.Init();
            Save((IContent)overview);

            for (int i = 0; i < 5; i++)
            {
                var item = this._contentRepository.GetDefault<GenericOverviewItemWithIconBlock>(assetsFolder.ContentLink);
                item = item.Init("FINANCE")
                          .AddIcon("60x60.png")
                          .AddLink(context.Homepage);

                if (itemHasDescription)
                    item = item.AddDescription();

                overview.AddItem(Save((IContent)item));
            }

            return Save((IContent)overview);
        }

        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
        }
    }
}