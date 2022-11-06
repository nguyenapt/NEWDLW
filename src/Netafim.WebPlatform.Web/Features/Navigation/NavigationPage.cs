using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    [ContentType(DisplayName = "Navigation Page", GUID = "D0DF226F-6F38-4F9D-A311-33C420BE3BCC")]
    [AvailableContentTypes(Availability.None, ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class NavigationPage : NoTemplatePageBase
    {
        [CultureSpecific]
        [Display(Order = 10, Description ="Specify the title of the navigation item.")]
        [UIHint(UIHint.LongString)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Name = "Navigation Link" , Description ="Specify the page of this navigation item, also be the root for the doormat under this item." , Order = 20)]
        [AllowedTypes(typeof(GenericContainerPage))]
        public virtual ContentReference Link { get; set; }

        [CultureSpecific]
        [Display(Name="Doormat Type", Order = 30)]
        [SelectOne(SelectionFactoryType = typeof(DoormatTypeFactory))]
        public virtual int DoormatType { get; set; }

        [CultureSpecific]
        [Display(Name ="View All Text",Description = "Text for [View All] link in the [Image Column Only] doormat", Order =40)]
        public virtual string ViewAllText { get; set; }
    }
}