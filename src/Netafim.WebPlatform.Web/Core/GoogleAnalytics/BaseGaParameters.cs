namespace Netafim.WebPlatform.Web.Core.GoogleAnalytics
{
    public class BaseGaParameters
    {
        /// <summary>
        /// Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Version.
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// Tracking ID / Property ID.
        /// </summary>
        public string TrackingId { get; set; }
        /// <summary>
        /// Anonymous Client ID.
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Hit Type. Ex: pageview, event
        /// </summary>
        public string HitType { get; set; }
    }
}