using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    [ContentType(DisplayName = "Hotspot Popup Node Page", GUID = "a13bc674-5b03-4b89-bab9-dc44e16ee999")]
    [AvailableContentTypes(ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class HotspotPopupNodePage : HotspotLinkNodePage
    {
        [CultureSpecific]
        [Display(Description = "Poup title", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Description = "Popup description", GroupName = SystemTabNames.Content, Order = 60)]
        public virtual string Description { get; set; }
    }
}