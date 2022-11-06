using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    public abstract class GenericOverviewItemBlock : ItemBaseBlock
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 15)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(PageData))]
        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual ContentReference Link { get; set; }
    }
}