using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.CropsOverview
{
    [ContentGenerator(Order = 100)]
    public class CropsOverviewContentGenerator : IContentGenerator
    {
        private const string Thumbnail1 = "~/Features/CropsOverview/Data/Demo/farm3.jpg";
        private const string Thumbnail2 = "~/Features/CropsOverview/Data/Demo/farm2.jpg";
        private const string Thumbnail3 = "~/Features/CropsOverview/Data/Demo/netmaize.png";

        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        public CropsOverviewContentGenerator(IContentRepository contentRepository,
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
            var cropExpertise = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            cropExpertise.PageName = "Crop expertise page";

            if (cropExpertise.Content?.FilteredItems.Any(IsCropsOverviewComponent) == true)
            {
                return;
            }

            Save(cropExpertise);

            var containerBlock = CreateCropContainer(cropExpertise);

            EnsureData(cropExpertise.ContentLink, containerBlock);

            var blockReference = Save((IContent)containerBlock);

            if (cropExpertise.Content == null)
            {
                cropExpertise.Content = new ContentArea();
            }

            cropExpertise.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = blockReference
            });

            Save(cropExpertise);
        }

        private CropsContainerBlock CreateCropContainer(GenericContainerPage cropExpertise)
        {
            var assetFolder = _contentAssetHelper.GetOrCreateAssetFolder(cropExpertise.ContentLink);
            var containerBlock = _contentRepository.GetDefault<CropsContainerBlock>(assetFolder.ContentLink);

            ((IContent)containerBlock).Name = "Crops overview container";
            containerBlock.Watermark = "Knowledge";
            containerBlock.VerticalText = "Agronimic Knowledge";
            containerBlock.Title = "Sharing agronomic knowledge and experience";
            containerBlock.Subtitle = "For more than 50 years, our experts share all their knowledge for growing and optimising your crops under your specific conditions.";
            containerBlock.Link = cropExpertise.ContentLink;
            return containerBlock;
        }

        private void EnsureData(ContentReference containerPageReference, CropsContainerBlock containerBlock)
        {
            containerBlock.Items = new ContentArea();
            containerBlock.Items.Items.Add(new ContentAreaItem()
            {
                ContentLink = CreateCropPage(containerPageReference, "Potato")
            });
            containerBlock.Items.Items.Add(new ContentAreaItem()
            {
                ContentLink = CreateCropPage(containerPageReference, "Cotton")
            });
            containerBlock.Items.Items.Add(new ContentAreaItem()
            {
                ContentLink = CreateCropPage(containerPageReference, "Sugarcane")
            });
        }

        private ContentReference CreateCropPage(ContentReference containerPageReference, string name)
        {
            var cropsDetailsPage = _contentRepository.GetDefault<CropsPage>(containerPageReference);
            cropsDetailsPage.Name = name;
            cropsDetailsPage.Title = name;
            cropsDetailsPage.PageName = name;
            cropsDetailsPage.URLSegment = _urlSegmentCreator.Create(cropsDetailsPage);
            cropsDetailsPage.Description = new XhtmlString(@"Are irrigation systems needed in areas with good amounts of rainfall?");
            cropsDetailsPage.Thumnbail = cropsDetailsPage.CreateBlob(Thumbnail1);

            return Save(cropsDetailsPage);
        }

        private ContentReference Save(IContent content)
        {
            return _contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private bool IsCropsOverviewComponent(ContentAreaItem arg)
        {
            var content = _contentRepository.Get<IContent>(arg.ContentLink);

            return content is CropsContainerBlock;
        }
    }
}