using System;
using System.Globalization;
using System.Linq;
using System.Web.Routing;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Dlw.EpiBase.Content.Cms
{
    // source: http://dodavinkeln.se/post/how-to-get-the-external-url-to-content
    public class DefaultExtendedUrlResolver : IExtendedUrlResolver
    {
        private UrlResolver _urlResolver;
        private ISiteDefinitionResolver _siteDefinitionResolver;

        public DefaultExtendedUrlResolver(UrlResolver urlResolver,
            ISiteDefinitionResolver siteDefinitionResolver)
        {
            _urlResolver = urlResolver;
            _siteDefinitionResolver = siteDefinitionResolver;
        }

        public string GetExternalUrl(ContentReference contentReference, CultureInfo contentLanguage, string action = null, object routeData = null)
        {
            if (contentReference == null) throw new ArgumentNullException(nameof(contentReference));

            var virtualPathArguments = new VirtualPathArguments
            {
                Action = action,
                ContextMode = ContextMode.Default,
                ForceCanonical = true
            };

            if (routeData != null)
            {
                virtualPathArguments.RouteValues = new RouteValueDictionary(routeData);
            }

            var result = _urlResolver.GetUrl(
                contentReference,
                contentLanguage.Name,
                virtualPathArguments);

            // HACK: Temprorary fix until GetUrl and ForceCanonical works as expected,
            // i.e returning an absolute URL even if there is a HTTP context that matches the content's site definition and host.
            Uri relativeUri;

            if (Uri.TryCreate(result, UriKind.RelativeOrAbsolute, out relativeUri))
            {
                if (!relativeUri.IsAbsoluteUri)
                {
                    var siteDefinition = _siteDefinitionResolver.GetByContent(contentReference, true, true);
                    var hosts = siteDefinition.GetHosts(contentLanguage, true).ToList();

                    var host = hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Primary)
                               ?? hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Undefined);

                    var basetUri = siteDefinition.SiteUrl;

                    if (host != null && host.Name.Equals("*") == false)
                    {
                        // Try to create a new base URI from the host with the site's URI scheme. Name should be a valid
                        // authority, i.e. have a port number if it differs from the URI scheme's default port number.
                        Uri.TryCreate(siteDefinition.SiteUrl.Scheme + "://" + host.Name, UriKind.Absolute, out basetUri);
                    }

                    var absoluteUri = new Uri(basetUri, relativeUri);

                    return absoluteUri.AbsoluteUri;
                }
            }

            return result;
        }
    }
}