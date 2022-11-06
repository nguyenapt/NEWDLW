using Netafim.WebPlatform.Web.Features.OfficeLocator;
using System.Web.Mvc;
using Netafim.WebPlatform.Web.Core.GoogleAnalytics;

namespace Netafim.WebPlatform.Web.Features.GoogleAnalytics
{
    public class GoogleAnalyticsController : Controller
    {
        private readonly IGoogleAnalyticsSettings _gaSettings;
        private readonly IGoogleSettings _googleSettings;
        public GoogleAnalyticsController(IGoogleAnalyticsSettings gaSettings, IGoogleSettings googleSettings)
        {
            _gaSettings = gaSettings;
            _googleSettings = googleSettings;
        }
        public ActionResult Index()
        {
            return PartialView("_analyticsTags");
        }

        public ActionResult GtmScript()
        {
            return PartialView("_gtmScript", _gaSettings.GtmId);
        }

        public ActionResult GtmNoScript()
        {
            return PartialView("_gtmNoScript", _gaSettings.GtmId);
        }
        
        public ActionResult GoogleMapScript()
        {
            return PartialView("_googleMapScript", _googleSettings.GoogleMapsApiKey);
        }
    }
}