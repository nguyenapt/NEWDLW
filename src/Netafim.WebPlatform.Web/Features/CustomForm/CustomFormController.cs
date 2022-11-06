using System.Web.Mvc;
using EPiServer;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Features.CustomForm.Models;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using Global = Netafim.WebPlatform.Web.Core.Templates.Global;

namespace Netafim.WebPlatform.Web.Features.CustomForm
{
    public class CustomFormController : BlockController<CustomFormContainerBlock>
    {
        public override ActionResult Index(CustomFormContainerBlock currentBlock)
        {
            var viewModel = new CustomFormContainerViewModel(currentBlock, ContainerViewModel());
         
            return PartialView(string.Format(Global.Constants.AbsoluteViewPathFormat, "CustomForm", currentBlock.GetOriginalType().Name), viewModel);
        }

        private RichTextContainerViewModel ContainerViewModel()
        {
            var parent = ControllerContext?.ParentActionViewContext?.Controller?.ViewData?.Model as RichTextContainerViewModel;

            return parent;
        }
    }
}
