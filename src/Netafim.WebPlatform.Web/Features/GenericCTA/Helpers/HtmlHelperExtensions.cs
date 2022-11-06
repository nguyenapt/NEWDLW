using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Get the additional attribute for the cta button
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString AdditionalCtaButtonAttributes<T>(this HtmlHelper<T> helper) where T : GenericCTABlock
        {
            var linkFactory = helper?.ViewData?.Model.GetUrlLinkFactory() as OverlayLinkUrlFactory;

            return linkFactory?.GetAdditionalCtaButtonAttributes(helper?.ViewData?.Model);
        }
    }
}