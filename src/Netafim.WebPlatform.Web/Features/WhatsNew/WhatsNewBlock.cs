using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    [ContentType(DisplayName = "What is New Block",
        GroupName = GroupNames.Overview,
        Description = "Component that display the what are new.",
        GUID = "784ab1b2-1fed-4271-bc46-1ae5b4f10407")]
    public class WhatsNewBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        public virtual string Title { get; set; }
        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);

        [AllowedTypes(new[] { typeof(SocialFeedsBlock), typeof(NewsEventOverviewBlock) })]
        public virtual ContentArea Items { get; set; }
    }
}