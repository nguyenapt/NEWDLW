using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{

    internal class ActionQueryAdapterBuilder : ActionQueryBaseBuilder<PipelineBaseBlock, ActionContext>
    {
        public ActionQueryAdapterBuilder(IContentLoader contentLoader,
            UrlResolver urlResolver,
            ICipher cipher) 
            : base(contentLoader, urlResolver, cipher)
        {
        }

        public override bool IsSatisfied(ActionContext actionContext)
        {
            var actionHandlers = ServiceLocator.Current.GetAllInstances<IActionHandler>();
            var actionHandler = actionHandlers?.FirstOrDefault(a => a.GetType() != typeof(ActionQueryAdapterBuilder) && a.IsSatisfied(actionContext));

            return actionHandler == null;
        }
    }
}