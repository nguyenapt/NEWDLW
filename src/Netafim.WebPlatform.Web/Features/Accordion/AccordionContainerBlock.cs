using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.Accordion
{
    [ContentType(DisplayName = "Accordion Container Block", GUID = "62bcafa6-21f9-4461-9aee-dbff0af5f2a3", Description = "", GroupName = GroupNames.Accordion)]
    public class AccordionContainerBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(Description = "Title of Accordion", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(AccordionItemBase))]
        [Display(Name = "Accordion items", Description = "Accordion items", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual ContentArea Items { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);
    }
}