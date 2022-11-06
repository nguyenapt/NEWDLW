using System;
using System.Configuration;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.CategorySelection
{
    //Source: https://getadigital.com/no/blogg/improved-categorylist-editor-descriptor-for-episerver/

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CategorySelectionAttribute : Attribute
    {
        /// 
        ///  ID of the root category.
        /// 
        public int RootCategoryId { get; set; }

        /// 
        /// Name of the root category.
        /// 
        public string RootCategoryName { get; set; }

        /// 
        /// The appSetting key containing the root category id to use.
        /// 
        public string RootCategoryAppSettingKey { get; set; }

        public int GetRootCategoryId()
        {
            var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();

            if (RootCategoryId > 0)
            {
                return RootCategoryId;
            }

            if (!string.IsNullOrWhiteSpace(RootCategoryName))
            {
                var category = categoryRepository.Get(RootCategoryName);

                if (category != null)
                {
                    return category.ID;
                }
            }

            if (!string.IsNullOrWhiteSpace(RootCategoryAppSettingKey))
            {
                string appSettingValue = ConfigurationManager.AppSettings[RootCategoryAppSettingKey];
                int rootCategoryId;

                if (!string.IsNullOrWhiteSpace(appSettingValue) && int.TryParse(appSettingValue, out rootCategoryId))
                {
                    return rootCategoryId;
                }
            }

            return categoryRepository.GetRoot().ID;
        }
    }
}