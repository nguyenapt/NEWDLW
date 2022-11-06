using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.RelatedContent
{
    [ContentType(DisplayName = "Related Content", GroupName = GroupNames.Listers, GUID = "e6e3a5da-04dd-46d2-9645-122f2ec0ffee", Description = "The component allows the webmaster to add visual links to content on the website.  This can be links to pages of the same and other content types.")]
    public class RelatedContentBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(IPreviewable))]
        public virtual ContentArea Items { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);
    }
}