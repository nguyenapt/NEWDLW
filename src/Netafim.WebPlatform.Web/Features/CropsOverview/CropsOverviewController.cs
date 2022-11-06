using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;

namespace Netafim.WebPlatform.Web.Features.CropsOverview
{
    [ContentOutputCache]
    public class CropsOverviewController : BasePageController<CropsPage>
    {
        public ActionResult Index(CropsPage currentPage)
        {
            return View(currentPage);
        }
    }
}