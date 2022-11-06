using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.Accordion
{
    public abstract class AccordionItemBase : ItemBaseBlock
    {
        [CultureSpecific]
        [Display(Description = "Question mark", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual XhtmlString Question { get; set; }

        [CultureSpecific]
        [Display(Description = "Answer the question", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString Answer { get; set; }
    }
}