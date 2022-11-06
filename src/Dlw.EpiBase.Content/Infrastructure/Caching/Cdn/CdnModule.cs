using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;

// source: https://github.com/bjuris/EPiServer.CdnSupport
// references:
//  https://github.com/torjue/EpiCdnHandler
//  https://github.com/mariajemaria/AzureCdn.Epi
//  https://gist.github.com/davidknipe/8a05d807dc73c198c51b

namespace Dlw.EpiBase.Content.Infrastructure.Caching.Cdn
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class CdnModule : IInitializableHttpModule
    {
        private static Regex _validateUrl = new Regex("^/[a-z0-9]{6}/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        public static object CdnRequest = new object();
        private static Lazy<string> _cdnUrl = new Lazy<string>(() => VirtualPathUtility.AppendTrailingSlash(ConfigurationManager.AppSettings["episerver:CdnExternalMediaUrl"]) ?? "/");

        // TODO add path bynder
        private static string[] _mediaPaths = new string[] { "contentassets", RouteCollectionExtensions.SiteAssetStaticSegment, RouteCollectionExtensions.GlobalAssetStaticSegment };

        private static Injected<IContentLoader> Loader;

        public void InitializeHttpEvents(HttpApplication application)
        {
            if (String.IsNullOrEmpty(_cdnUrl.Value)) { return; }
            application.BeginRequest += new EventHandler(OnBeginRequest);
        }

        public void Initialize(EPiServer.Framework.Initialization.InitializationEngine context)
        {
            if (String.IsNullOrEmpty(_cdnUrl.Value)) { return; }
            context.Locate.Advanced.GetInstance<IContentRouteEvents>().CreatedVirtualPath += ContentRoute_CreatedVirtualPath;
        }

        public void Uninitialize(EPiServer.Framework.Initialization.InitializationEngine context)
        {
        }

        private static void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext c = ((HttpApplication)sender).Context;
            if (!_validateUrl.IsMatch(c.Request.Path))
            {
                return;
            }
            string newPath = c.Request.Path.Substring(8);
            if (_mediaPaths.Any(p => newPath.StartsWith(p)))
            {
                c.Items[CdnRequest] = c.Request.Path.Substring(1, 6);
                // known issue: https://github.com/bjuris/EPiServer.CdnSupport/issues/1
                c.RewritePath("/" + newPath, String.Empty, c.Request.QueryString.ToString(), true);
            }
        }

        private static void ContentRoute_CreatedVirtualPath(object sender, UrlBuilderEventArgs e)
        {
            if (e.RequestContext.GetContextMode() != ContextMode.Default || !_mediaPaths.Any(p => e.UrlBuilder.Path.StartsWith(p)))
            {
                return;
            }

            var contentLink = e.RouteValues[RoutingConstants.NodeKey] as ContentReference;
            IContent content;
            if (Loader.Service.TryGet(contentLink, out content) && ((ISecurable)content).GetSecurityDescriptor().HasAccess(PrincipalInfo.AnonymousPrincipal, AccessLevel.Read))
            {
                e.UrlBuilder.Uri = new Uri(_cdnUrl + Unique(content) + "/" + e.UrlBuilder.Path, UriKind.RelativeOrAbsolute);
            }
        }

        public static string Unique(IContent content)
        {
            var d = ((IChangeTrackable)content).Saved;
            int hash = 17;
            unchecked
            {
                hash = hash * 23 + d.Month;
                hash = hash * 23 + d.Day;
                hash = hash * 23 + d.Minute;
                hash = hash * 23 + d.Second;
            }
            return hash.ToString("x6");
        }
    }
}