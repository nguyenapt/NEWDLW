using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    [ContentType(DisplayName = "Product Category Listing block",
        GroupName = GroupNames.Listers,
        GUID = "d7895aa8-ea24-4339-835c-bdd94ed5e6b4")]
    public class ProductCategoryListingBlock : ListingBaseBlock, IComponent
    {
        [Display(Order = 10)]
        [CultureSpecific]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Order = 20)]
        [UIHint(UIHint.LongString)]
        public virtual string SubTitle { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName();
    }
}