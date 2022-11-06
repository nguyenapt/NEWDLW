using EPiServer.Core;
using EPiServer.Core.Html;

namespace Dlw.EpiBase.Content.Infrastructure.Extensions
{
    public static class XHtmlStringExtensions
    {
        public static string ToTextString(this XhtmlString xhtmlString)
        {
            if (xhtmlString == null) return null;

            var html = xhtmlString.ToString();

            return TextIndexer.StripHtml(html, html.Length);
        }
    }
}