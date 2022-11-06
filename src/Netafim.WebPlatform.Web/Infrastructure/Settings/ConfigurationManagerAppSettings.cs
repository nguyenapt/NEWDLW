using Dlw.EpiBase.Content;
using Dlw.EpiBase.Content.Infrastructure;

namespace Netafim.WebPlatform.Web.Infrastructure.Settings
{
    public class ConfigurationManagerAppSettings : BaseConfigurationManagerSettings, IAppSettings
    {
        private string Environment => GetAppSetting<string>("Environment").ToLowerInvariant();

        public bool IsLocal => Environment == EpiEnvironment.Local.ToLowerInvariant();

        public bool IsProduction => Environment == EpiEnvironment.Production.ToLowerInvariant();
    }
}