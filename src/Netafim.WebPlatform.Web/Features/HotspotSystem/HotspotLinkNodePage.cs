using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    [ContentType(DisplayName = "Hotspot Link Node Page", GUID = "b530efe1-c4a4-4145-8b8a-e4c8720d6aae")]
    [AvailableContentTypes(ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class HotspotLinkNodePage : PageData
    {
        [CultureSpecific]
        [Display(Name = "Name", Description = "Name of node", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string NodeName { get; set; }

        [CultureSpecific]
        [Display(Description = "Link", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual ContentReference Link { get; set; }

        [CultureSpecific]
        [Display(Name = "Coordinates(X)", Description = "Coordinates X", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual double CoordinatesX { get; set; }

        [CultureSpecific]
        [Display(Name = "Coordinates(Y)", Description = "Coordinates Y", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual double CoordinatesY { get; set; }
    }
}