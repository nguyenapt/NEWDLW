namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public interface IBynderSettings
    {
        bool Enabled { get; }

        string ProviderName { get; }

        string ConsumerKey { get; }

        string ConsumerSecret { get; }

        string Token { get; }

        string TokenSecret { get; }

        string ApiBaseUrl { get; }

        int MaxAssetsToFetch { get; }
    }
}