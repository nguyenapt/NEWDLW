using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    [ContentType(DisplayName = "Generic overview item with thumbnail", GUID = "44868452-CAAB-4B9E-B12F-C93C09A56A84", GroupName = GroupNames.Overview)]
    public class GenericOverviewItemWithThumbnailBlock : GenericOverviewItemBlock, ICTAComponent
    {
        [CultureSpecific]
        [AllowedTypes(typeof(ImageData))]
        [Display(Name = "Image (620x452)", GroupName = SystemTabNames.Content, Order = 10)]
        [ImageMetadata(620, 452)]
        public virtual ContentReference Thumbnail { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString Description { get; set; }

        [CultureSpecific]
        [Display(Name ="Link Text", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string LinkText { get; set; }        
    }
}