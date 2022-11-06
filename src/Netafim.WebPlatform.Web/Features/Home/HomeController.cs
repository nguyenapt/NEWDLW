using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;

namespace Netafim.WebPlatform.Web.Features.Home
{
    [ContentOutputCache]
    public class HomeController : BasePageController<HomePage>
    {
        public ActionResult Index(HomePage currentPage)
        {
            return View(currentPage);
        }
    }
}