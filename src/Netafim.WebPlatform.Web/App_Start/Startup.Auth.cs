using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Infrastructure.Owin.Security;
using Owin;

namespace Netafim.WebPlatform.Web
{
    public partial class Startup
    {
        public void ConfigureSecurityShield(IAppBuilder app)
        {
            var appSettings = ServiceLocator.Current.GetInstance<ISecurityShieldSettings>();

            if (appSettings.Enabled)
            {
                app.UseSecurityShield(new SecurityShieldOptions
                {
                    WhiteListedAddresses = appSettings.WhitelistedIps,
                    AADClientId = appSettings.AadClientId,
                    AADInstance = appSettings.AadInstance,
                    AADTenant = appSettings.AadTenant
                });
            }
        }
    }
}