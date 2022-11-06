using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using EPiServer.Logging;
using Newtonsoft.Json;

namespace Netafim.WebPlatform.Web.Core.GoogleAnalytics
{
    /// <summary>
    /// refer: https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
    /// </summary>
    public class GoogleAnalytics : IGoogleAnalytics
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(GoogleAnalytics));
        private readonly IGoogleAnalyticsSettings _googleAnalyticsSettings;

        public GoogleAnalytics(IGoogleAnalyticsSettings googleAnalyticsSettings)
        {
            _googleAnalyticsSettings = googleAnalyticsSettings;
        }
        public void TrackEvent(GaEventParameters p)
        {
            var data = GetEventData(p);
            Post(data);
        }

        private NameValueCollection GetEventData(GaEventParameters p)
        {
            var data = GetBaseData(p);
            data["t"] = "event";
            data["v"] = "1";
            if (!string.IsNullOrEmpty(p.EventCategory)) data["ec"] = HttpUtility.UrlEncode(p.EventCategory);
            if (!string.IsNullOrEmpty(p.EventAction)) data["ea"] = HttpUtility.UrlEncode(p.EventAction);
            if (!string.IsNullOrEmpty(p.EventLabel)) data["el"] = p.EventLabel;
            return data;
        }

        private NameValueCollection GetBaseData(BaseGaParameters p)
        {
            var data = new NameValueCollection
            {
                ["tid"] = _googleAnalyticsSettings.GATrackingId,
                ["url"] = _googleAnalyticsSettings.GaUrl,
                ["cid"] = p.ClientId
            };

            return data;
        }

        private void Post(NameValueCollection data)
        {
            var d = data.AllKeys.ToDictionary(k => k, k => data[k]);
            var jsonData = JsonConvert.SerializeObject(d, Formatting.Indented);
            _logger.Information($"Calling GoogleMeasurementProtocol data: {jsonData}");

            using (var wc = new WebClient())
            {
                wc.UploadValues(data["url"], "POST", data);
            }
        }
    }
}