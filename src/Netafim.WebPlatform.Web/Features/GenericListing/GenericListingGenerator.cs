using System.Collections.Generic;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.GenericListing
{
    public class GenericListingGenerator : IContentGenerator
    {
        private const string MediaFolder = "~/Features/GenericListing/Data/Demo/{0}";
        private readonly IContentRepository _contentRepository;
        private readonly CategoryRepository _categoryRepository;
        private const string Category1 = "Protected crops";
        private const string Category2 = "Orchards";
        private const string Category3 = "Open fields";

        public GenericListingGenerator(
            IContentRepository contentRepository, 
            CategoryRepository categoryRepository)
        {
            this._contentRepository = contentRepository;
            _categoryRepository = categoryRepository;
        }

        public void Generate(ContentContext context)
        {
            CreateCategoryIfNotExists(Category1, Category1, 1);
            CreateCategoryIfNotExists(Category2, Category2, 2);
            CreateCategoryIfNotExists(Category3, Category3, 3);

            CreateCropExpertisePage(context);
        }

        private void CreateCategoryIfNotExists(string name, string description, int order)
        {
            var category = _categoryRepository.Get(name);
            if (category != null) return;

            var newCategory = new Category(name, description)
            {
                Available = true,
                Parent = _categoryRepository.GetRoot(),
                SortOrder = order
            };

            _categoryRepository.Save(newCategory);
        }

        private void CreateCropExpertisePage(ContentContext context)
        {
            var cropExpertise = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            cropExpertise.PageName = "Crop Expertise";

            Save(cropExpertise);

            cropExpertise.Content = cropExpertise.Content ?? new ContentArea();

            GenerateListingBlock(cropExpertise);

            GenerateChildrenPage(cropExpertise, 20);

            Save(cropExpertise);
        }

        private void GenerateListingBlock(GenericContainerPage cropExpertisePage)
        {
            var cropOverviewFiler = this._contentRepository.GetDefault<GenericListingBlock>(cropExpertisePage.ContentLink).CreateWritableClone() as GenericListingBlock;
            cropOverviewFiler.Watermark = "Crop experince";
            ((IContent)cropOverviewFiler).Name = "Crop overview filter";

            cropExpertisePage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)cropOverviewFiler) });
        }
        
        private void GenerateChildrenPage(GenericContainerPage cropExpertisePage, int numberOfChildren)
        {
            for (int i = 0; i < numberOfChildren; i++)
            {
                GenerateCropPage(cropExpertisePage, "page ", i);
            }
        }

        private void GenerateCropPage(GenericContainerPage cropExpertisePge, string posFixName, int sequentialNumber)
        {
            posFixName += sequentialNumber;

            var categoryProject1 = _categoryRepository.Get(Category1);
            var categoryProject2 = _categoryRepository.Get(Category2);
            var categoryProject3 = _categoryRepository.Get(Category3);

            var crop = this._contentRepository.GetDefault<GenericContainerPage>(cropExpertisePge.ContentLink).CreateWritableClone() as GenericContainerPage;
            crop.PageName = $"Crop {posFixName}";
            crop.Title = $"ALMOND {posFixName}";
            crop.Description = new XhtmlString(@"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras egestas vestibulum turpis et imperdiet. Pellentesque maximus nec diam eu rutrum. Suspendisse potenti.</p>");
            crop.Thumnbail = cropExpertisePge.CreateBlob(string.Format(MediaFolder, "165x165.png"));

            var newPageRef = Save(crop);

            var newPage = _contentRepository.Get<GenericContainerPage>(newPageRef);
            var writableClone = newPage.CreateWritableClone();
            if (sequentialNumber % 1 == 0) AddCategory(writableClone, categoryProject1.ID);
            if (sequentialNumber % 2 == 0) AddCategory(writableClone, categoryProject2.ID);
            if (sequentialNumber % 5 == 0) AddCategory(writableClone, categoryProject3.ID);

            Save(writableClone);
        }

        private void AddCategory(PageData currentPage, int categoryId)
        {
            if (currentPage.Category == null)
            {
                currentPage.Category = new CategoryList(new List<int>(categoryId));
            }
            else
            {
                currentPage.Category.Add(categoryId);
            }
        }

        private ContentReference Save(IContent content)
        {
            return _contentRepository.Save(content, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
        }
    }
}