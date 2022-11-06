using System.Net;
using EPiServer.Logging;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Dlw.EpiBase.Content.Infrastructure.AppInsights
{
    public class SuppressNotFoundRequestTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(SuppressNotFoundRequestTelemetryProcessor));

        private ITelemetryProcessor Next { get; set; }

        public SuppressNotFoundRequestTelemetryProcessor(ITelemetryProcessor next)
        {
            this.Next = next;
        }

        public void Process(ITelemetry item)
        {
            var requestTelemetry = item as RequestTelemetry;

            if (requestTelemetry?.ResponseCode == ((int)HttpStatusCode.NotFound).ToString())
            {
                _logger.Warning($"404: {requestTelemetry.Url}");
                return;
            }

            Next.Process(item);
        }
    }
}