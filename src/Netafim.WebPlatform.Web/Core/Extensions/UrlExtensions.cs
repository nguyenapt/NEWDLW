using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Rendering;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class UrlExtensions
    {
        public static IContent GetContent(this string url, IContentLoader contentLoader)
        {
            if (string.IsNullOrEmpty(url)) return null;
            var contentGuid = PermanentLinkUtility.GetGuid(url);

            IContent content = null;
            if(contentGuid != Guid.Empty && contentLoader.TryGet(contentGuid, out content))
            {
                return content;
            }

            return null;
        }
        
        public static MvcHtmlString LinkTarget(this string url)
        {
            return !string.IsNullOrWhiteSpace(url) ? new Url(url).LinkTarget() : MvcHtmlString.Empty;
        }

        public static MvcHtmlString LinkTarget(this Url url)
        {
            var urlRenderings = ServiceLocator.Current.GetAllInstances<IUrlRendering>();

            if (urlRenderings == null)
                throw new Exception("No IUrlRendering is registered.");

            var rendering = urlRenderings.FirstOrDefault(r => r.IsSatisfied(url));

            if (rendering == null)
                throw new Exception($"Can not find any service to render the url {url}.");

            return rendering.LinkTarget(url);
        }
        
        public static string ImageUrl(this string url, Expression<Func<IContentData, ContentReference>> imageExpression)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            if (imageExpression == null)
                throw new ArgumentNullException(nameof(imageExpression));

            var memberExpression = imageExpression.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException(nameof(imageExpression), "The expression is not a member expression");

            var imageMetaData = memberExpression.Member.GetCustomAttribute<ImageMetadataAttribute>();

            return imageMetaData != null
                ? $"{url}?height={imageMetaData.Height}&width={imageMetaData.Width}&mode={imageMetaData.Mode.ToImageResizerMode()}"
                : url;
        }
    }
}