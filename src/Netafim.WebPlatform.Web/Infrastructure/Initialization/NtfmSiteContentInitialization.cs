using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise.Internal;
using EPiServer.Forms.Implementation.Elements;
using EPiServer.Framework;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Features.Settings;
using Netafim.WebPlatform.Web.Features.SuccessStory;
using Product = Netafim.WebPlatform.Web.Features.SuccessStory.Product;

namespace Netafim.WebPlatform.Web.Infrastructure.Initialization
{
    [ModuleDependency(typeof(DefaultSiteContentInitialization))]
    public class NtfmSiteContentInitialization : BaseDefaultSiteContentInitialization
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(NtfmSiteContentInitialization));

        protected override void EnsureSiteConfiguration(IServiceLocator serviceLocator)
        {
            EnsureSettingsPage(serviceLocator);
            EnsureCategories(serviceLocator);
        }

        protected override ContentReference CreateDefaultContent(IServiceLocator serviceLocator)
        {
            var homePage = EnsureHomePageInAllCultures(serviceLocator);

            EnsureDemoContent(serviceLocator, homePage);

            return homePage;
        }

        private static void EnsureSettingsPage(IServiceLocator serviceLocator)
        {
            var contentRepository = serviceLocator.GetInstance<IContentRepository>();

            var settingsPage = contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();

            if (settingsPage == null)
            {
                var urlSegmentCreator = serviceLocator.GetInstance<IUrlSegmentCreator>();

                settingsPage = contentRepository.GetDefault<SettingsPage>(ContentReference.RootPage);
                settingsPage.PageName = "Settings";
                settingsPage.URLSegment = urlSegmentCreator.Create(settingsPage);
                
                var leadForm = contentRepository.GetDefault<FormContainerBlock>(ContentReference.RootPage);
                ((IContent)leadForm).Name = "Lead form name";
                settingsPage.LeadForm = contentRepository.Save(((IContent)leadForm), EPiServer.DataAccess.SaveAction.Publish, AccessLevel.NoAccess);

                contentRepository.Save(settingsPage, EPiServer.DataAccess.SaveAction.Publish, AccessLevel.NoAccess);
            }
        }

        private static void EnsureCategories(IServiceLocator serviceLocator)
        {
            var categoryRepository = serviceLocator.GetInstance<CategoryRepository>();

            var root = categoryRepository.GetRoot();

            //TODO: create the required categories below & remove this sample code.

            //var productCategory = EnsureCategory<Product>(root, categoryRepository);
            //EnsureCategory<Type>(productCategory, categoryRepository);
            //EnsureCategory<Filling>(productCategory, categoryRepository);

            //EnsureCategory<Tag>(root, categoryRepository);

            //EnsureCategory<Faq>(root, categoryRepository);

            var projectCategory = EnsureCategory<Product>(root, categoryRepository);
            EnsureCategory<Big>(projectCategory, categoryRepository, true);
            //TODO: end-todo
        }

        private static Category EnsureCategory<T>(Category parent, CategoryRepository categoryRepository, bool selectable = false) where T : Epi.Shell.CategorySelection.Category
        {
            var categoryName = typeof(T).Name;

            var category = categoryRepository.Get(categoryName);
            if (category != null) return category;

            var productCategory = new Category(parent, categoryName)
            {
                Selectable = selectable,
                Description = categoryName
            };

            categoryRepository.Save(productCategory);

            return productCategory;
        }

        private static ContentReference EnsureHomePageInAllCultures(IServiceLocator serviceLocator)
        {
            var contentRepository = serviceLocator.GetInstance<IContentRepository>();
            var urlSegmentCreator = serviceLocator.GetInstance<IUrlSegmentCreator>();

            var homePage = contentRepository.GetDefault<HomePage>(ContentReference.RootPage);
            homePage.PageName = "Homepage";
            homePage.URLSegment = urlSegmentCreator.Create(homePage);

            var content = contentRepository.Save(homePage, EPiServer.DataAccess.SaveAction.Publish, AccessLevel.NoAccess);

            var repository = serviceLocator.GetInstance<ILanguageBranchRepository>();

            var enabledLangBranches = repository.ListEnabled().Where(x => !x.LanguageID.Equals("en"));

            foreach (var lang in enabledLangBranches)
            {
                var createdPage = contentRepository.CreateLanguageBranch<PageData>(content.Copy().ToPageReference(), new LanguageSelector(lang.LanguageID));
                createdPage.Name = "Homepage";
                createdPage.URLSegment = urlSegmentCreator.Create(createdPage);

                contentRepository.Save(createdPage, EPiServer.DataAccess.SaveAction.Publish, AccessLevel.NoAccess);
            }

            return homePage.ContentLink;
        }

        private void EnsureDemoContent(IServiceLocator serviceLocator, ContentReference homePage)
        {
            var contentGeneratorService = serviceLocator.GetInstance<IContentGeneratorService>();

            var contentContext = new ContentContext() { Homepage = homePage };
            var messages = contentGeneratorService.Generate(contentContext);

            foreach (var message in messages)
            {
                foreach (var generatorMessage in message.Messages)
                {
                    _logger.Information($"{message.Generator} -> {generatorMessage}");
                }
            }
        }
    }
}