using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    [ContentType(DisplayName = "Hotspot Container Page", GUID = "331f9ed2-6c94-43cb-95de-8725852a1eac", Description = "The component allows the webmaster to create a visual overview of a system or process", GroupName = GroupNames.Containers)]
    [AvailableContentTypes(Include = new[] { typeof(IHotspotTemplate) }, ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class HotspotContainerPage : NoTemplatePageBase
    {
    }
}