using System.Web.Mvc;
using Microsoft.ApplicationInsights.Extensibility;

namespace Netafim.WebPlatform.Web.Features.ApplicationInsights
{
    public class ApplicationInsightsController : Controller
    {
        public ActionResult Index()
        {
            if (TelemetryConfiguration.Active == null) return new EmptyResult();

            var key = TelemetryConfiguration.Active.InstrumentationKey;
            if (string.IsNullOrEmpty(key)) return new EmptyResult();

            var model = new ApplicationInsightsModel
            {
                InstrumentationKey = key
            };

            return PartialView("_applicationInsights", model);
        }
    }
}