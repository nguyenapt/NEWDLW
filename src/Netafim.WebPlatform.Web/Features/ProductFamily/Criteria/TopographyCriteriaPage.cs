using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.ProductFamily.Criteria
{
    [ContentType(DisplayName = "Topography Criteria page", GUID = "ec6a3ef9-5dc3-44dd-b8f9-a8a05d52e33d")]
    public class TopographyCriteriaPage : CriteriaPage
    {
        [CultureSpecific]
        [AllowedTypes(typeof(ImageData))]
        [Display(Name = "Image (230x40)", Order = 10, Description = "Image will be displayed on the list of Product Family page")]
        [ImageMetadata(230, 40)]
        public virtual ContentReference Image { get; set; }
    }
}