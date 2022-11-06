using System;
using System.Web;

namespace Dlw.EpiBase.Content.Infrastructure.Web
{
    public static class HttpRequestBaseExtensions
    {
        public static bool IsAzureBot(this HttpRequestBase httpRequestBase)
        {
            if (httpRequestBase == null) throw new ArgumentNullException(nameof(httpRequestBase));

            // can be null, eg during some azure infra calls
            if (httpRequestBase.UserAgent == null) return false;

            if (httpRequestBase.UserAgent.Equals(Maintenance.Constants.AlwaysOnUserAgent, StringComparison.OrdinalIgnoreCase) ||
                httpRequestBase.UserAgent.Equals(Maintenance.Constants.ApplicationInitializationUserAgent, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}