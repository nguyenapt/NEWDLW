using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.CookieMessage
{
    public class CookieMessageController : Controller
    {
        private readonly ICookieMessageSettings _cookieMessageSettings;

        public CookieMessageController(ICookieMessageSettings cookieMessageSettings)
        {
            _cookieMessageSettings = cookieMessageSettings;
        }

        public ActionResult Index()
        {
            return PartialView("_cookieMessage", new CookieMessageViewModel()
            {
                CookieLink = _cookieMessageSettings.CookieLink
            });
        }
    }
}