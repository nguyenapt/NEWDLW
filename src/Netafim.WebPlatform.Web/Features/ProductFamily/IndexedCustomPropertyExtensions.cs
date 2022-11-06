using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public static class IndexedCustomPropertyExtensions
    {
        public static int[] CriteriaIdCollection(this ProductCategoryPage page)
        {
            if (page ==null || page.CriteriaCollection.IsNullOrEmpty()) { return new int[0]; }
            return page.CriteriaCollection.FilteredItems?.Select(x => x.ContentLink.ID)?.ToArray();
        }

        public static int[] ProductCategoryIdCollection(this ProductFamilyPage page)
        {
            if (page == null || page.ProductCategories.IsNullOrEmpty()) { return new int[0]; }
            return page.ProductCategories.FilteredItems?.Select(x => x.ContentLink.ID)?.ToArray();
        }

        public static int[] PropertyIdCollection(this ProductFamilyPage page)
        {
            if (page == null || page.PropertyCollection.IsNullOrEmpty()) { return new int[0]; }
            return page.PropertyCollection.FilteredItems?.Select(x => x.ContentLink.ID)?.ToArray();
        }
    }   

}