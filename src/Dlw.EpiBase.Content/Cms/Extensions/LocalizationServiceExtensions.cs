using System;
using System.Text;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;

namespace Dlw.EpiBase.Content.Cms.Extensions
{
    public static class LocalizationServiceExtensions
    {
        public const string CategoryLabelKeyPrefix = "Category";

        public static string TranslateCategory(this LocalizationService localizationService, int categoryId)
        {
            if (localizationService == null) throw new ArgumentNullException(nameof(localizationService));

            var category = ServiceLocator.Current.GetInstance<CategoryRepository>().Get(categoryId);

            if (category == null) throw new ArgumentException($"No category found with id '{categoryId}'.", nameof(categoryId));

            var fullPath = BuildFullPath(category);

            return localizationService.GetString($"{CategoryLabelKeyPrefix}.{fullPath}");
        }

        private static object BuildFullPath(Category category)
        {
            var key = new StringBuilder(category.Name);

            while (category.Parent != null && category.Parent.ID != 1) // 1 == Root
            {
                key.Insert(0, ".");
                key.Insert(0, category.Parent.Name);

                category = category.Parent;
            }

            return key.ToString();
        }
    }
}