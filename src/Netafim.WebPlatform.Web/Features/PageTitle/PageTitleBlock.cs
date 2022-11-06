using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.PageTitle
{
    [ContentType(DisplayName = "Page Title Block", GUID = "84858bff-a356-4618-ac43-fab36808e2fe", Description = "Display as header of page", GroupName = GroupNames.Listers)]
    public class PageTitleBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(Description = "Title of page", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        [Display(Description = "Description of page", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string Description { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);
    }
}