using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    [ContentType(DisplayName = "Navigation Container Page", GUID = "5430AEA8-7626-4E96-9D80-4A482A5AE64B", GroupName = GroupNames.Containers)]
    [AvailableContentTypes(Include = new[] {  typeof(NavigationPage) }, ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class NavigationContainerPage : NoTemplatePageBase
    {
    }
}