using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.ProductFamilyQuickLink
{
    public class ProductFamilyQuickLinkModel
    {
        public string Title { get; set; }

        public IEnumerable<ProductFamilyQuickLinkItem> Items { get; set; }
    }

    public class ProductFamilyQuickLinkItem
    {
        public string Text { get; set; }

        public string Link { get; set; }
    }
}