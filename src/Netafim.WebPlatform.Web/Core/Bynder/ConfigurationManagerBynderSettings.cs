using Dlw.EpiBase.Content.Infrastructure;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public class ConfigurationManagerBynderSettings : BaseConfigurationManagerSettings, IBynderSettings
    {
        public bool Enabled => GetAppSetting<bool>("Bynder.Enabled");

        public string ProviderName => GetAppSetting<string>("Bynder.Provider.Name");

        public string ConsumerKey => GetAppSetting<string>("Bynder.Repository.ConsumerKey");

        public string ConsumerSecret => GetAppSetting<string>("Bynder.Repository.ConsumerSecret");

        public string Token => GetAppSetting<string>("Bynder.Repository.Token");

        public string TokenSecret => GetAppSetting<string>("Bynder.Repository.TokenSecret");

        public string ApiBaseUrl => GetAppSetting<string>("Bynder.Repository.ApiBaseUrl");

        public int MaxAssetsToFetch => GetAppSetting<int>("Bynder.Repository.MaxAssetsToFetch");
    }
}