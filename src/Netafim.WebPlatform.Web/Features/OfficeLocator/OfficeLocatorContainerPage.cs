using EPiServer.DataAnnotations;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator
{
    [ContentType(DisplayName = "Office location container", GroupName = GroupNames.Containers, GUID = "89f7c857-8fdd-4984-a800-db84f023f686", Description = "Group the office locator template")]
    [AvailableContentTypes(Include = new[] { typeof(OfficeLocatorPage) }, ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class OfficeLocatorContainerPage : NoTemplatePageBase
    {

    }
}