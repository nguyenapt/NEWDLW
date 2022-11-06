using System.Text.RegularExpressions;
using System.Web;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Dlw.EpiBase.Content.Infrastructure.AppInsights
{
    public class IgnoreNotFoundExceptionTelemetryProcessor : ITelemetryProcessor
    {
        private static readonly Regex _notFoundExceptionRegex = new Regex(@"The controller for path '\/[^']+' was not found or does not implement IController\.");
        private static readonly Regex _fileNotFoundExceptionRegex = new Regex(@"The file '\/.+' does not exist\.");

        private ITelemetryProcessor Next { get; set; }

        public IgnoreNotFoundExceptionTelemetryProcessor(ITelemetryProcessor next)
        {
            this.Next = next;
        }

        public void Process(ITelemetry item)
        {
            var exceptionTelemetry = item as ExceptionTelemetry;

            if (exceptionTelemetry?.Exception is HttpException)
            {
                if (_notFoundExceptionRegex.IsMatch(exceptionTelemetry?.Exception.Message) ||
                    _fileNotFoundExceptionRegex.IsMatch(exceptionTelemetry?.Exception.Message))
                {
                    return;
                }
            }

            Next.Process(item);
        }
    }
}