using System.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;

namespace Netafim.WebPlatform.Web.Features.FloatingCTA
{
    public class FloatingCtaController : BasePageController<PageBase>
    {
        private readonly IFloatingSharing _floatingSharing;
        public FloatingCtaController(IFloatingSharing floatingSharing)
        {
            _floatingSharing = floatingSharing;
        }

        public ActionResult Index(PageBase currentPage)
        {
            var model = currentPage.HideFloatingSharing ? string.Empty : _floatingSharing.ShareaholicSiteId;
            return PartialView("_floatingSharing", model);
        }
    }
}