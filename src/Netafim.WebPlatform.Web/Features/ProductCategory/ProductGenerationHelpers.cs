using Castle.Core.Internal;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.ProductFamily;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    public static class ProductGenerationHelpers
    {
        private const string MediaFolder = "~/Features/ProductCategory/Data/Images/{0}";
        public static T AddItemsToMainContent<T>(this T page, IEnumerable<ContentReference> itemlinks) where T : GenericContainerPage
        {
            var editablePage = page.CreateWritableClone() as T;
            if (editablePage == null || itemlinks.IsNullOrEmpty()) { return editablePage; }
            editablePage.Content = editablePage.Content ?? new ContentArea();
            foreach (var contentAreaItemLink in itemlinks)
            {
                editablePage.Content.Items.Add(new ContentAreaItem()
                {
                    ContentLink = contentAreaItemLink
                });
            }
            return editablePage;
        }

        public static T AddAssocitatesToProductCategory<T>(this T page, IEnumerable<ContentReference> productCateLinks) where T : ProductFamilyPage
        {
            page = page.CreateWritableClone() as T;
            if (page == null || productCateLinks.IsNullOrEmpty()) { return page; }
            page.ProductCategories = page.Content ?? new ContentArea();
            foreach (var contentAreaItemLink in productCateLinks)
            {
                page.ProductCategories.Items.Add(new ContentAreaItem()
                {
                    ContentLink = contentAreaItemLink
                });
            }
            return page;
        }

        public static T AddCriteriaToFamily<T>(this T page, IEnumerable<ContentReference> criteriaLinks) where T : ProductFamilyPage
        {
            page = page.CreateWritableClone() as T;
            if (page == null || criteriaLinks.IsNullOrEmpty()) { return page; }
            page.PropertyCollection = page.Content ?? new ContentArea();
            foreach (var contentAreaItemLink in criteriaLinks)
            {
                page.PropertyCollection.Items.Add(new ContentAreaItem()
                {
                    ContentLink = contentAreaItemLink
                });
            }
            return page;
        }

        public static T AddCriteriaToProductCategory<T>(this T page, IEnumerable<ContentReference> criteriaContainerRefs) where T : ProductCategoryPage
        {
            page = page.CreateWritableClone() as T;
            if (page == null || criteriaContainerRefs.IsNullOrEmpty()) { return page; }
            page.CriteriaCollection = page.Content ?? new ContentArea();
            foreach (var contentAreaItemLink in criteriaContainerRefs)
            {
                page.CriteriaCollection.Items.Add(new ContentAreaItem()
                {
                    ContentLink = contentAreaItemLink
                });
            }
            return page;
        }

        public static T AddThumbnail<T>(this T page, string fileName) where T : GenericContainerPage
        {
            page = page.CreateWritableClone() as T;
            page.Thumnbail = page.CreateBlob(string.Format(MediaFolder, fileName));
            return page;
        }        

        public static T AddDescription<T>(this T page, string desc = null) where T : GenericContainerPage
        {
            page = page.CreateWritableClone() as T;
            page.Description = !string.IsNullOrEmpty(desc) ? new XhtmlString(desc)
                : new XhtmlString("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s");
            return page;
        }
        public static T AddResultDescription<T>(this T block, string desc = null) where T : FamilyMatrixBlock
        {
            block = block.CreateWritableClone() as T;
            block.SearchResultDescription = !string.IsNullOrEmpty(desc) ? desc
                : "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s";
            return block;
        }
        public static T AddTitle<T>(this T block, string title = null) where T : FamilyMatrixBlock
        {
            block = block.CreateWritableClone() as T;
            block.Title = !string.IsNullOrEmpty(title) ? title
                : "There are many variations of passages of Lorem Ipsum available";
            return block;
        }
        public static T AddSubTitle<T>(this T block, string subTitle = null) where T : FamilyMatrixBlock
        {
            block = block.CreateWritableClone() as T;
            block.SubTitle = !string.IsNullOrEmpty(subTitle) ? subTitle
                : "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old";
            return block;
        }
    }
}