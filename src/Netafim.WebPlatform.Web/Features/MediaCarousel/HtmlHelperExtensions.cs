using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    public static class HtmlHelperExtensions
    {
        public static string UrlContent<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IMediaCarousel>> expression)
        {
            var model = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            var factories = ServiceLocator.Current.GetInstance<IEnumerable<IUrlLinkFactory>>();

            var factory = factories?.FirstOrDefault(f => f.IsSatisfy(model));

            return factory?.CreateLink(model);
        }

        public static string ImageUrlWithCarouselMode(this UrlHelper urlHelper, ICarouselMode container, IMediaCarousel carousel)
        {
            var factories = ServiceLocator.Current.GetInstance<IEnumerable<ICarouselContainerModeFactory>>();
            var factory = factories?.FirstOrDefault(f => f.IsSatisfy(container));

            return factory?.CreateImageUrl(urlHelper, carousel);
        }
    }
}