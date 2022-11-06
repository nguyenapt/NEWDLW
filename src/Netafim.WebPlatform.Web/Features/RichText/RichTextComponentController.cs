using EPiServer;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using System;
using System.Web.Mvc;
using System.Linq;
using EPiServer.Core;
using System.Collections;
using System.Collections.Generic;
using EPiServer.Logging;
using Global = Netafim.WebPlatform.Web.Core.Templates.Global;

namespace Netafim.WebPlatform.Web.Features.RichText
{
    public class RichTextComponentController : PartialContentController<IRichTextColumnComponent>
    {
        private ILogger _logger = LogManager.GetLogger();
        private readonly IContentRepository _contentRepository;

        public RichTextComponentController(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public override ActionResult Index(IRichTextColumnComponent currentContent)
        {
            object viewModel = CreateViewModel(currentContent);
            
            return PartialView(string.Format(Global.Constants.AbsoluteViewPathFormat, "Richtext", currentContent.GetOriginalType().Name), viewModel);
        }

        private object CreateViewModel(IRichTextColumnComponent currentContent)
        {
            object viewModel = null;

            try
            {
                var type = typeof(RichTextComponentViewModel<>).MakeGenericType(currentContent.GetOriginalType());
                viewModel = Activator.CreateInstance(type, currentContent, GetRichtextContainerViewModel());
            }
            catch(Exception ex)
            {
                _logger.Error("Cant not be create the RichTextComponentViewmodel dynamically.", ex);
                throw new Exception("Cant not be create the RichTextComponentViewmodel dynamically.", ex);
            }

            return viewModel;
        }
        
        private RichTextContainerViewModel GetRichtextContainerViewModel()
        {
            // Get the function that get the current index of this component on the content area that passed from container view
            return ControllerContext?.ParentActionViewContext?.Controller?.ViewData?.Model as RichTextContainerViewModel;
        }
    }
}