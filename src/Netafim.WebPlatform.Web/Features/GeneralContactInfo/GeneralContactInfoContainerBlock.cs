using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.GeneralContactInfo
{
    [ContentType(DisplayName = "General Contact Information container", GroupName = GroupNames.Overview, GUID = "fb4d0e35-e45a-46b1-920d-d00b7026d95b", Description = "Display the general information")]
    public class GeneralContactInfoContainerBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Description = "Container contains the rich text component", Order = 30)]
        [AllowedTypes(typeof(GeneralContactInfoItemBlock))]
        public virtual ContentArea Items { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);
    }
}