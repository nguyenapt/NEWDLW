using System.Web;
using EPiServer.Logging;

namespace Netafim.WebPlatform.Web.Core.GoogleAnalytics
{
    public static class HttpRequestBaseExtensions
    {
        public static string GetGAClientId(this HttpRequestBase request)
        {
            ILogger _logger = LogManager.GetLogger(typeof(HttpRequestBaseExtensions));
            
            var gaCookie = request.Cookies["_ga"];

            if (gaCookie == null)
            {
                _logger.Error("HttpRequestBaseExtensions cookie _ga cannot be found");
                return string.Empty;
            }

            if (string.IsNullOrEmpty(gaCookie.Value))
            {
                _logger.Error("HttpRequestBaseExtensions cookie _ga is empty");
                return string.Empty;
            }

            var values = gaCookie.Value.Split('.');

            if (values.Length != 4)
            {
                _logger.Error($"GoogleAnalyticsCookieParser cookie with length: {values.Length} cannot be parsed: {gaCookie.Value}");
                return string.Empty;
            }

            return $"{values[2]}.{values[3]}";
        }
    }
}