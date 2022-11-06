using Dlw.EpiBase.Content.Infrastructure;

namespace Netafim.WebPlatform.Web.Infrastructure.Owin.Security
{
    public class ConfigurationManagerSecurityShieldSettings : BaseConfigurationManagerSettings, ISecurityShieldSettings
    {
        public bool Enabled => GetAppSetting<bool>("SecurityShield.Enabled");

        public string WhitelistedIps => GetAppSetting<string>("SecurityShield.WhitelistedIps", false);

        public string AadClientId => GetAppSetting<string>("SecurityShield.AadClientId");

        public string AadInstance => GetAppSetting<string>("SecurityShield.AadInstance");

        public string AadTenant => GetAppSetting<string>("SecurityShield.AadTenant");
    }
}