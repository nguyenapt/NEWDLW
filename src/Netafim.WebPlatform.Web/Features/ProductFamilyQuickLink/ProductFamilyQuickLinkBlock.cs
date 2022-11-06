using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.ProductFamilyQuickLink
{
    [ContentType(DisplayName = "Product Family Quick Links Block", GUID = "4d5402c9-4a4a-46d1-a832-4b5978361f68", Description = "")]
    public class ProductFamilyQuickLinkBlock : ItemBaseBlock
    {
        [CultureSpecific]
        [Display(Description = "Quick links", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }
    }
}