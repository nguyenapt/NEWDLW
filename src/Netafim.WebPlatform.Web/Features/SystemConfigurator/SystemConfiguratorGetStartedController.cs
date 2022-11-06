using System.Collections.Generic;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Cms;
using EPiServer;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.ViewModels;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{

    public class SystemConfiguratorGetStartedController : PipelineBlockBaseController<SystemConfiguratorGetStarterBlock, StarterActionContext>
    {
        private readonly IUserContext _userContext;

        public SystemConfiguratorGetStartedController(IContentLoader contentLoader, 
            IEnumerable<IActionHandler> actionHandlers,
            IUserContext userContext) 
            : base(contentLoader, actionHandlers)
        {
            this._userContext = userContext;
        }

        public override ActionResult Index(SystemConfiguratorGetStarterBlock currentContent)
        {
            // flow
                // ask for name and email
                // push to next step through querystring

            var viewModel = new SystemConfiguratorGetStartedViewModel(currentContent)
            {
                ActionUrl = $"/{_userContext.CurrentLanguage}{Url.Action(nameof(TakeAction))}"
            };

            return PartialView(currentContent.GetDefaultFullViewName(), viewModel);
        }        
    }
}