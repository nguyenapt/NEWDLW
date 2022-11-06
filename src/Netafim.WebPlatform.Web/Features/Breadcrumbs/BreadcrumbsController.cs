using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public class BreadcrumbsController : Controller
    {
        public ActionResult Index()
        {            
            return PartialView("Breadcrumbs");
        }
    }
}