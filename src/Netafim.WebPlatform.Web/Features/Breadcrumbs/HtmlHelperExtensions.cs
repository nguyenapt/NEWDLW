using Castle.Core.Internal;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Features.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Netafim.WebPlatform.Web.Features.Breadsrumbs
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString Breadscrumbs(
           this HtmlHelper helper,
           ContentReference rootLink, Dictionary<BreadcrumbItemType, Func<BreadcrumbsItem, HelperResult>> itemTemplates = null,
           bool includeRoot = false,
           bool requireVisibleInMenu = true,
           bool requirePageTemplate = true)
        {
            if (itemTemplates == null || !itemTemplates.Any(x => x.Key.Equals(BreadcrumbItemType.CurrentItem))
                || !itemTemplates.Any(x => x.Key.Equals(BreadcrumbItemType.NormalLinkItem))
                || !itemTemplates.Any(x => x.Key.Equals(BreadcrumbItemType.NormalTextItem)))
            {
                itemTemplates = GetDefaultItemTemplates(helper);
            }

            var currentContentLink = helper.ViewContext.RequestContext.GetContentLink();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

            var pagePath = contentLoader.GetAncestors(currentContentLink)
                .FilterForDisplay(requirePageTemplate, requireVisibleInMenu)
              .Reverse()
              .Select(x => x.ContentLink)
              .SkipWhile(x => !x.CompareToIgnoreWorkID(rootLink))
              .ToList();

            if (pagePath.IsNullOrEmpty()) { return new MvcHtmlString(string.Empty); }
            if (!includeRoot && pagePath.Contains(rootLink)) { pagePath.Remove(rootLink); }

            pagePath.Add(currentContentLink);
            var breadcrumbsItems = new List<BreadcrumbsItem>();
            foreach (var item in pagePath)
            {
                PageData page;
                if (!contentLoader.TryGet(item, out page)) continue;

                breadcrumbsItems.Add(new BreadcrumbsItem(page));
            }

            var buffer = new StringBuilder();
            using (var writer = new StringWriter(buffer))
            {
                foreach (var menuItem in breadcrumbsItems)
                {
                    Func<BreadcrumbsItem, HelperResult> template = GetTempalte(menuItem, itemTemplates, helper, currentContentLink);
                    template(menuItem).WriteTo(writer);
                }
            }

            return new MvcHtmlString(buffer.ToString());
        }

        private static Func<BreadcrumbsItem, HelperResult> GetTempalte(BreadcrumbsItem menuItem, Dictionary<BreadcrumbItemType, Func<BreadcrumbsItem, HelperResult>> itemTemplates, HtmlHelper helper, ContentReference currentContentLink)
        {
            if (itemTemplates == null || !itemTemplates.Any() || menuItem == null || menuItem.Page == null) { return GetDefaultTemplate(helper); }
            if (itemTemplates.ContainsKey(BreadcrumbItemType.DefaultItem)) { return itemTemplates[BreadcrumbItemType.DefaultItem]; }
            if (menuItem.Page.ContentLink.Equals(currentContentLink)) { return itemTemplates[BreadcrumbItemType.CurrentItem]; }
            if (menuItem.Page.HasTemplate()) { return itemTemplates[BreadcrumbItemType.NormalLinkItem]; }
            return itemTemplates[BreadcrumbItemType.NormalTextItem];
        }

        private static Dictionary<BreadcrumbItemType, Func<BreadcrumbsItem, HelperResult>> GetDefaultItemTemplates(HtmlHelper helper)
        {
            return new Dictionary<BreadcrumbItemType, Func<BreadcrumbsItem, HelperResult>>() { { BreadcrumbItemType.DefaultItem, GetDefaultTemplate(helper) } };
        }

        private static Func<BreadcrumbsItem, HelperResult> GetDefaultTemplate(HtmlHelper helper)
        {
            return x => new HelperResult(writer => writer.Write(helper.PageLink(x.Page)));
        }


    }
    public enum BreadcrumbItemType
    {
        NormalLinkItem,
        NormalTextItem,
        CurrentItem,
        DefaultItem
    }
}