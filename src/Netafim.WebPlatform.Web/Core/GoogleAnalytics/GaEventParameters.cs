namespace Netafim.WebPlatform.Web.Core.GoogleAnalytics
{
    public class GaEventParameters : BaseGaParameters
    {
        /// <summary>
        /// Event Category. Required
        /// </summary>
        public string EventCategory { get; set; }
        /// <summary>
        /// Event Action. Required.
        /// </summary>
        public string EventAction { get; set; }
        /// <summary>
        /// Event label.
        /// </summary>
        public string EventLabel { get; set; }
        /// <summary>
        /// Event value.
        /// </summary>
        public string FieldsObject { get; set; }
    }
}