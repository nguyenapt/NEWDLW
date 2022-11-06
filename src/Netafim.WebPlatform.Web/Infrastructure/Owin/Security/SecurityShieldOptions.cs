using System;

namespace Netafim.WebPlatform.Web.Infrastructure.Owin.Security
{
    public class SecurityShieldOptions
    {
        private const string DefaultAadRootPath = "/aadint";
        private const string DefautAuthenticationType = "SecurityShieldCookie";

        /// <summary>
        /// The id of the Azure Ad Client application
        /// </summary>
        public string AADClientId { get; set; }

        /// <summary>
        /// The url used to login with your AAD
        /// </summary>
        public string AADInstance { get; set; }

        /// <summary>
        /// The Azure Ad Tenant name
        /// </summary>
        public string AADTenant { get; set; }

        /// <summary>
        /// Path that will be used for AAD requests, and should be included in the ignore path settings
        /// of sitecore / exclude from authorization in web.config (eg forms auth). Default is "/aadint".
        /// </summary>
        public string AADIntegrationPath { get; set; }

        /// <summary>
        /// Name that will be used for the authentication cookie. Default is "SecurityShieldCookie".
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// The time, in hours, it takes before you need to re-authenticate. Default is 8hrs.
        /// </summary>
        public TimeSpan ExpireTimeSpan { get; set; }

        /// <summary>
        /// Whether or not to use sliding expiration for the authentication cookie.
        /// </summary>
        public bool SlidingExpiration { get; set; }

        /// <summary>
        /// Semicolon seperated list of ip-addresses that are allowed access.
        /// Ip-ranges can also be provided by seperating them by a slash. 
        /// ex: 13.95.9.122;84.199.105.0/84.199.105.255
        /// </summary>
        public string WhiteListedAddresses { get; set; }

        public SecurityShieldOptions()
        {
            AADIntegrationPath = DefaultAadRootPath;
            AuthenticationType = DefautAuthenticationType;
            ExpireTimeSpan = new TimeSpan(8, 0, 0);
            SlidingExpiration = true;
        }
    }
}