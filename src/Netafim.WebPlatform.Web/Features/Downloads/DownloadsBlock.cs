using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.Downloads
{
    [ContentType(DisplayName = "Downloads Block", GUID = "980fc90b-cbe8-4d05-92b1-0886dba11732", Description = "")]
    public class DownloadsBlock : RichTextComponent
    {
        [CultureSpecific]
        [Display(
        Name = "Icon image (36x36)",
        Description = "Specify the icon for downloads items.",
        GroupName = SystemTabNames.Content,
        Order = 1)]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(36, 36)]
        public virtual ContentReference Icon { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Download items",
            Description = "Container for the download items.",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [AllowedTypes(new[] { typeof(GenericMedia), typeof(ImageData), typeof(VideoData) })]
        public virtual ContentArea DownloadItems { get; set; }
    }
}