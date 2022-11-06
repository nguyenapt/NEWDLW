using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Extensions method to render the image with image resizer parameter
        /// default image resizer will crop the image to fit the size
        /// </summary>
        /// <param name="url"></param>
        /// <param name="imageReference"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string CropImageUrl(this UrlHelper url, ContentReference imageReference, int width, int height)
        {
            if (imageReference != null)
            {
                return url.ImageUrl(imageReference, width, height, "crop");
            }
            return string.Empty;
        }

        /// <summary>
        /// Extensions method to render the image with image resizer parameter
        /// </summary>
        /// <param name="url"></param>
        /// <param name="imageReference"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string ImageUrl(this UrlHelper url, ContentReference imageReference, int width, int height, string mode)
        {
            var imageUrl = url.ContentUrl(imageReference);

            return $"{imageUrl}?height={height}&width={width}&mode={mode}";
        }
        
        /// <summary>
        /// Extensions method to render the image with image resizer parameter
        /// </summary>
        /// <param name="url"></param>
        /// <param name="imageExpression"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static string ImageUrl(this UrlHelper url, IContentData contentData, Expression<Func<IContentData, ContentReference>> imageExpression)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            if (contentData == null)
                throw new ArgumentNullException(nameof(contentData));

            if (imageExpression == null)
                throw new ArgumentNullException(nameof(imageExpression));

            var memberExpression = imageExpression.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException(nameof(imageExpression), "The expression is not a member expression");

            var imageReference = imageExpression.Compile().Invoke(contentData);

            var imageMetaData = memberExpression.Member.GetCustomAttribute<ImageMetadataAttribute>();

            return imageReference != null && imageMetaData != null ? url.ImageUrl(imageReference, imageMetaData.Width, imageMetaData.Height, imageMetaData.Mode.ToImageResizerMode()) : url.ContentUrl(imageReference);
        }
        
        /// <summary>
        /// Convert from resize mode enum to ImageResizer mode
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string ToImageResizerMode(this ResizeMode mode)
        {
            switch (mode)
            {
                case ResizeMode.Crop:
                    return "crop";
                default:
                    return "crop";
            }
        }
    }
}
