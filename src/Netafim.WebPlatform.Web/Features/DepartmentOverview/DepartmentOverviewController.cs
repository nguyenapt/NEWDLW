using EPiServer.Web.Mvc;
using System.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.DepartmentOverview
{

    public class DepartmentOverviewController : BlockController<DepartmentOverviewBlock>
    {
        public override ActionResult Index(DepartmentOverviewBlock currentContent)
        {
            var parentModel = ControllerContext?.ParentActionViewContext?.Controller?.ViewData?.Model as DepartmentOverviewContainerBlock;

            var viewModel = new DepartmentOverviewViewModel(currentContent, parentModel?.FilterComponentAnchorId);

            return PartialView(currentContent.GetDefaultViewName(), viewModel);
        }
    }
}