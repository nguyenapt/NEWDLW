using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Infrastructure.Maintenance.Warmup;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Logging;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Dlw.EpiBase.Content.Infrastructure.Maintenance
{
    public class MaintenanceController : Controller
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(MaintenanceController));

        private readonly IClient _searchClient;
        private readonly UrlResolver _urlResolver;
        private readonly ISiteDefinitionResolver _siteDefinitionResolver;

        public MaintenanceController(IClient searchClient, UrlResolver urlResolver, ISiteDefinitionResolver siteDefinitionResolver)
        {
            _searchClient = searchClient;
            _urlResolver = urlResolver;
            _siteDefinitionResolver = siteDefinitionResolver;
        }

        public ActionResult Warmup()
        {
            _logger.Information($"Start {nameof(MaintenanceController)}.{nameof(Warmup)}");

            try
            {
                var itemsToFetch = 5;
                var pageToFetch = 0;
                var content = GetContent(pageToFetch, itemsToFetch);

                do
                {
                    foreach (var contentReference in content)
                    {
                        string url = null;
                        try
                        {
                            url = GetAbsoluteUrl(contentReference, HttpContext);

                            ExecuteRequest(url);
                        }
                        catch (Exception e)
                        {
                            _logger.Error($"Error occured while trying to warmup content '{contentReference}' (url '{url}').", e);
                        }
                    }

                    pageToFetch++;
                    content = GetContent(pageToFetch, itemsToFetch);
                } while (content.Any());

                _logger.Information($"Completed {nameof(MaintenanceController)}.{nameof(Warmup)}");

            }
            catch (Exception e)
            {
                _logger.Error($"Error occured during {nameof(MaintenanceController)}.{nameof(Warmup)}", e);

                throw;
            }

            return new ContentResult() { Content = "Done" };
        }

        private string GetAbsoluteUrl(ContentReference contentReference, HttpContextBase httpContext)
        {
            // TODO investigate Global.UrlRewriteProvider.ConvertToExternal(urlBuilder, contentReference, UTF8Encoding.UTF8);

            var url = _urlResolver.GetUrl(contentReference, "en", new VirtualPathArguments { ForceCanonical = true });

            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri) return url;

            var uriBuilder = new UriBuilder(httpContext.Request.Url)
            {
                Path = url
            };

            return uriBuilder.Uri.AbsoluteUri;
        }

        private void ExecuteRequest(string url)
        {
            var request = WebRequest.Create(url);
            request.Timeout = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var statusCode = response.StatusCode;

                stopwatch.Stop();
                _logger.Information($"Requested '{url}' with code '{statusCode}' (duration: {stopwatch.Elapsed.TotalSeconds}s)");
            }
        }

        // Get all content to preload in every published language
        private SearchResults<ContentReference> GetContent(int pageToFetch, int pageSize)
        {
            var result = _searchClient.Search<IPreload>()
                .CurrentlyPublished()
                .Select(x => x.ContentLink)
                .Skip(pageToFetch * pageSize)
                .Take(pageSize)
                .GetResult();

            return result;
        }
    }

    internal static class Extensions
    {
        // TODO review workaround (reverse engineered epi extension)
        // move to dedicated base assembly for only web project usage?
        public static ITypeSearch<T> CurrentlyPublished<T>(this ITypeSearch<T> search) where T : IContentData
        {
            var filterBuilder = search.Client.BuildFilter<IVersionable>().And(x => x.Status.Match((Enum)VersionStatus.Published));

            return search.Filter<T>(filterBuilder);
        }
    }
}