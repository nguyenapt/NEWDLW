using EPiServer.DataAnnotations;

namespace Netafim.WebPlatform.Web.Core.Templates
{

    [ContentType(DisplayName = "Container page without template to group content", GroupName = GroupNames.Containers, GUID = "134a1a87-85cf-46c9-a2e1-620a2b5ffa25")]
    [AvailableContentTypes(Include = new [] { typeof(NoTemplatePageBase)}, ExcludeOn = new[] { typeof(GenericContainerPage) })]
    public class NoTemplateContainerPage : NoTemplatePageBase
    {

    }
}