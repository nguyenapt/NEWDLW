using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DbLocalizationProvider;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{
    public class PipelineBlockBaseController<TPipeline, TActionContext> : PartialContentController<TPipeline>
        where TPipeline : PipelineBaseBlock
        where TActionContext : ActionContext
    {
        protected readonly IContentLoader ContentLoader;
        protected readonly IEnumerable<IActionHandler> ActionHandlers;

        public PipelineBlockBaseController(IContentLoader contentLoader,
            IEnumerable<IActionHandler> actionHandlers)
        {
            this.ContentLoader = contentLoader;
            this.ActionHandlers = actionHandlers;
        }

        public override ActionResult Index(TPipeline currentContent)
        {
            return PartialView(currentContent.GetDefaultFullViewName(), currentContent);
        }

        [HttpPost]
        public virtual ActionResult TakeAction(TActionContext actionContext)
        {
            this.ExtendContext(actionContext);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", LocalizationProvider.Current.GetString(() => Labels.ValidationInputMessage));

                var content = this.ContentLoader.Get<IContent>(new ContentReference(actionContext.ContentId)) as TPipeline;

                return PartialView(content.GetDefaultFullViewName(), content);
            }

            var actionHandler = this.ActionHandlers.FirstOrDefault(a => a.IsSatisfied(actionContext));

            if (actionHandler == null)
                throw new System.Exception($"Can not find any handler with current content {actionContext.GetType()}");

            var next = actionHandler.TakeAction(actionContext);

            return Redirect(next);
        }

        protected virtual void ExtendContext(TActionContext actionContext)
        {

        }
    }
}