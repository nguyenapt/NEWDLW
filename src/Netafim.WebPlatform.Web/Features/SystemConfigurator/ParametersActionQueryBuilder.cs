using EPiServer;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{

    public class ParametersActionQueryBuilder : ActionQueryBaseBuilder<SystemConfiguratorParametersBlock, ParametersActionContext>
    {
        public ParametersActionQueryBuilder(IContentLoader contentLoader, UrlResolver urlResolver, ICipher cipher)
            : base(contentLoader, urlResolver, cipher)
        {
        }
    }
}