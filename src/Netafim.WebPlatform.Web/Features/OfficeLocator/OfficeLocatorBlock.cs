using EPiServer.Core;
using Netafim.WebPlatform.Web.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator
{
    [ContentType(DisplayName = "Netafim Worldwide block", GroupName = GroupNames.Locator, GUID = "0f66a0a2-a842-4cab-9cf6-532a2b4139cf", Description = "Netafim Worldwide")]
    public class OfficeLocatorBlock : ItemBaseBlock, IComponent
    {
        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName();
    }
}