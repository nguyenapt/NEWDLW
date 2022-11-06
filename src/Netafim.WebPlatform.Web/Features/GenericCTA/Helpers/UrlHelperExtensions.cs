using EPiServer.ServiceLocation;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Extensions method to help view can build the link with anchor value
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public static string ContentUrl(this UrlHelper urlHelper, GenericCTABlock block)
        {
            IUrlLinkFactory linkFactory = block.GetUrlLinkFactory();

            return linkFactory.CreateLink(urlHelper, block);
        }

        /// <summary>
        /// Extension method tho help view can check the content is email or not
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static bool IsEmailCTA<TModel>(this HtmlHelper<TModel> helper) where TModel : GenericCTABlock
        {
            var linkFactory = helper?.ViewData?.Model?.GetUrlLinkFactory();

            return linkFactory != null && linkFactory is EmailUrlLinkFactory;
        }

        /// <summary>
        /// Extension method that help view can check the content is downloadable or not
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool IsDownloadableContent<TModel>(this HtmlHelper<TModel> helper) where TModel : GenericCTABlock
        {
            var linkFactory = helper?.ViewData?.Model?.GetUrlLinkFactory();

            return linkFactory != null && linkFactory is MediaLinkUrlFactory;
        }

        /// <summary>
        /// Extension method that get the media name
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string DownloadableContentName<TModel>(this HtmlHelper<TModel> helper) where TModel : GenericCTABlock
        {
            var mediaFactory = helper?.ViewData?.Model?.GetUrlLinkFactory() as MediaLinkUrlFactory;

            return mediaFactory?.GetMediaName(helper.ViewData.Model.Link);
        }

        /// <summary>
        /// Extension method that help view can detect the cta is overlay or not
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool IsOverlayCTA<TModel>(this HtmlHelper<TModel> helper) where TModel : GenericCTABlock
        {
            var linkFactory = helper?.ViewData?.Model?.GetUrlLinkFactory();

            return linkFactory != null && linkFactory is OverlayLinkUrlFactory;
        }
    }
}