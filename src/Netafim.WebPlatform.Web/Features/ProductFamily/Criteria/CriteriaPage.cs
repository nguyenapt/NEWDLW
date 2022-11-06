using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.ProductFamily.Criteria
{
    [ContentType(DisplayName = "Criteria page", GUID = "c5026cf7-f2b7-4a8a-9175-437e94272ef5")]
    public class CriteriaPage : NoTemplatePageBase, IProductFamilyProperty
    {        
        [CultureSpecific]
        [Display(Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(ImageData))]
        [Display(Name = "Image (230x40)", Order = 10, Description = "Image will be displayed on the list of Product Family page")]
        [ImageMetadata(230, 40)]
        public virtual ContentReference Image { get; set; }

        public string Key => this.ContentLink.ID.ToString();

        public string Value => string.IsNullOrEmpty(Title) ? Name : Title;
    }
}