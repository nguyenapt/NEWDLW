using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    [ContentType(DisplayName = "Generic overview item with icon", GUID = "9cc4e55d-0afc-4d93-a0fd-09763e349587", GroupName = GroupNames.Overview)]
    public class GenericOverviewItemWithIconBlock : GenericOverviewItemBlock
    {
        [CultureSpecific]
        [AllowedTypes(typeof(ImageData))]
        [Display(Name = "Image (60x60)", GroupName = SystemTabNames.Content, Order = 10)]
        [ImageMetadata(60, 60)]
        public virtual ContentReference Icon { get; set; }      
   
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Description { get; set; }
    }
}