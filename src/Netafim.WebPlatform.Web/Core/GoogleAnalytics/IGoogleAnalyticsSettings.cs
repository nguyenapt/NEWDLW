namespace Netafim.WebPlatform.Web.Core.GoogleAnalytics
{
    public interface IGoogleAnalyticsSettings
    {
        string GtmId { get; }
        string GaUrl { get; }
        string GATrackingId { get; }
        string PrefixGAEventCategory { get; }
    }
}