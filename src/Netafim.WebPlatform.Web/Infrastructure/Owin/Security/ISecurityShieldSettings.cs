namespace Netafim.WebPlatform.Web.Infrastructure.Owin.Security
{
    public interface ISecurityShieldSettings
    {
        bool Enabled { get; }

        string WhitelistedIps { get; }

        string AadClientId { get; }

        string AadInstance { get; }

        string AadTenant { get; }
    }
}