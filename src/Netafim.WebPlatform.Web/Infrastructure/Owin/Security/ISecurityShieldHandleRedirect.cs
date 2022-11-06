using Microsoft.Owin.Security.Notifications;

namespace Netafim.WebPlatform.Web.Infrastructure.Owin.Security
{
    public interface ISecurityShieldHandleRedirect
    {
        /// <summary>
        /// Return true if request is handled and no redirect to AAD is required.
        /// </summary>
        bool RedirectToIdentityProvider<TMessage, TAuthenticationOptions>(RedirectToIdentityProviderNotification<TMessage, TAuthenticationOptions> context);
    }
}