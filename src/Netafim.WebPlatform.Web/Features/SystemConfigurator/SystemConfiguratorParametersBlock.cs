using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{

    [ContentType(DisplayName = "System Configurator Parameters block", GUID = "a2171c8c-d6cc-440b-8154-15b96e1b54b4", GroupName = GroupNames.Configurator)]
    public class SystemConfiguratorParametersBlock : PipelineBaseBlock
    {
    }

    [ContentType(DisplayName = "System Configurator Result block", GUID = "c73b8497-809c-4e66-b90d-aaf54cdd92ca", GroupName = GroupNames.Configurator)]
    public class SystemConfiguratorResultBlock : PipelineBaseBlock
    {
    }
}