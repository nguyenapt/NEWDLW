using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public static class ContentExtensions
    {
        public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> contents, bool requirePageTemplate = false,
           bool requireVisibleInMenu = false)
           where T : IContent
        {
            var accessFilter = new FilterAccess();
            var publishedFilter = new FilterPublished();
            contents = contents.Where(x => !publishedFilter.ShouldFilter(x) && !accessFilter.ShouldFilter(x));
            if (requirePageTemplate)
            {
                var templateFilter = ServiceLocator.Current.GetInstance<FilterTemplate>();
                templateFilter.TemplateTypeCategories = TemplateTypeCategories.Page;
                contents = contents.Where(x => !templateFilter.ShouldFilter(x));
            }
            if (requireVisibleInMenu)
            {
                contents = contents.Where(x => VisibleInMenu(x));
            }
            return contents;
        }

        private static bool VisibleInMenu(IContent content) => (content as PageData)?.VisibleInMenu ?? true;
    }
}