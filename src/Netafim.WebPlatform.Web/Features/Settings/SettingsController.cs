using System.Web.Mvc;
using EPiServer.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.Settings
{
    public class SettingsController : PageController<SettingsPage>
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}