using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.OnThisPage
{
    [ContentType(DisplayName = "On This Page Block", GUID = "801b50d6-b0bf-4a1f-a5ee-c018fb66ebe7", Description = "Component that allows the user to switch easily between different product categories or families", GroupName = GroupNames.Infrastructure)]
    public class OnThisPageBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string SubTitle { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);
    }
}