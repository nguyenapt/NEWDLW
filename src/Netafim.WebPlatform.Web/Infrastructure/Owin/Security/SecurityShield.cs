using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace Netafim.WebPlatform.Web.Infrastructure.Owin.Security
{
    public class SecurityShield : OwinMiddleware
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(SecurityShield));

        private readonly SecurityShieldOptions _options;
        private static readonly List<IPAddress> WhiteListedAddresses = new List<IPAddress>();
        private static readonly List<IPAddressRange> WhiteListedRanges = new List<IPAddressRange>();

        public SecurityShield(OwinMiddleware next, SecurityShieldOptions options) : base(next)
        {
            _options = options;
            var addresses = options.WhiteListedAddresses?.Split(';');

            if (addresses == null)
                return;

            foreach (var address in addresses)
            {
                if (address.Contains("/"))
                {
                    IPAddressRange range;
                    if (IPAddressRange.TryParse(address, out range))
                    {
                        WhiteListedRanges.Add(range);
                    }
                }
                else
                {
                    IPAddress ipAddress;
                    if (IPAddress.TryParse(address, out ipAddress))
                    {
                        WhiteListedAddresses.Add(ipAddress);

                        _logger.Information($"Whitelisted IP '{ipAddress}'");
                    }
                }
            }
        }

        public override async Task Invoke(IOwinContext context)
        {
            //Ignore local requests
            if (context.Request.IsLocal()) // NEXT replace with Uri.IsLoopback?
            {
                await Next.Invoke(context);
                return;
            }

            // Don't check security on aad integration path
            if (context?.Request != null && context.Request.Uri.LocalPath.StartsWith(_options.AADIntegrationPath))
            {
                await Next.Invoke(context);
                return;
            }

            // Whitelisted?
            if (IsWhiteListed(context))
            {
                await Next.Invoke(context);
                return;
            }

            var authenticationResult = context?.Authentication.AuthenticateAsync(_options.AuthenticationType).Result;

            // Authenticated?
            if (authenticationResult?.Identity != null && authenticationResult.Identity.IsAuthenticated)
            {
                await Next.Invoke(context);
                return;
            }

            // Redirect to login url, which will trigger a redirect to ad login
            if (context?.Request != null)
            {
                var redirectUrl = $"{_options.AADIntegrationPath}/login?ReturnUrl={context.Request.Uri.LocalPath}";
                context.Response.Redirect(redirectUrl);
            }
        }

        private bool IsWhiteListed(IOwinContext context)
        {
            IPAddress toVerify;
            if (!IPAddress.TryParse(context.Request.RemoteIpAddress, out toVerify))
                return false;

            if (IsWhiteListed(toVerify))
                return true;

            // Whitelisted based on x-forwarded-for header?
            var xforwardedforheader = context.Request.Headers["X-Forwarded-For"];

            if (string.IsNullOrEmpty(xforwardedforheader))
                return false;

            IPAddress fromXForwardedForHeader;

            var valid = IPAddress.TryParse(xforwardedforheader, out fromXForwardedForHeader);

            return valid && IsWhiteListed(fromXForwardedForHeader);
        }

        private bool IsWhiteListed(IPAddress toVerify)
        {
            return WhiteListedAddresses.Any(ip => ip.Equals(toVerify)) || WhiteListedRanges.Any(range => range.Contains(toVerify));
        }
    }

    public static class SecurityShieldExtensions
    {
        public static IAppBuilder UseSecurityShield(this IAppBuilder app, SecurityShieldOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (string.IsNullOrEmpty(options.AuthenticationType))
            {
                throw new ArgumentNullException(nameof(options.AuthenticationType));
            }
            if (string.IsNullOrEmpty(options.AADIntegrationPath))
            {
                throw new ArgumentNullException(nameof(options.AADIntegrationPath));
            }

            app.SetDefaultSignInAsAuthenticationType(options.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = options.AuthenticationType,
                AuthenticationMode = AuthenticationMode.Passive,
                ExpireTimeSpan = options.ExpireTimeSpan,
                SlidingExpiration = options.SlidingExpiration
            });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    UseTokenLifetime = false,
                    ClientId = options.AADClientId,

                    Authority = string.Format(CultureInfo.InvariantCulture, options.AADInstance, options.AADTenant),
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            context.Response.Write(context.Exception.ToString());

                            return Task.FromResult(0);
                        },
                        RedirectToIdentityProvider = context =>
                        {
                            var redirectHandlers = ServiceLocator.Current.GetAllInstances<ISecurityShieldHandleRedirect>();

                            foreach (var handler in redirectHandlers)
                            {
                                if (handler.RedirectToIdentityProvider(context))
                                {
                                    context.HandleResponse();

                                    return Task.FromResult(0);
                                }
                            }

                            var redirectUri = $"{context.Request.Scheme}://{context.Request.Host}{options.AADIntegrationPath}/return";
                            context.ProtocolMessage.RedirectUri = redirectUri;
                            context.ProtocolMessage.PostLogoutRedirectUri = redirectUri;

                            return Task.FromResult(0);
                        }
                    }
                });

            app.Use<SecurityShield>(options);

            app.Map($"{options.AADIntegrationPath}/login", config =>
            {
                // Trigger unauthorized so that active authentication will redirect to active directory.
                config.Run(context =>
                {
                    var returnUrl = context.Request.Query["ReturnUrl"] ?? "/";

                    context.Authentication.Challenge(new AuthenticationProperties { RedirectUri = returnUrl, IsPersistent = true },
                        OpenIdConnectAuthenticationDefaults.AuthenticationType);

                    context.Response.StatusCode = 401;

                    // Middleware will redirect us instead of using this output.
                    return context.Response.WriteAsync(string.Empty);
                });
            });

            return app;
        }
    }
}