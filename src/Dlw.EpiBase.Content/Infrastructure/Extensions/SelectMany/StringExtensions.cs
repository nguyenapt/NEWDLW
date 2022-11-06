using System;
using System.Web;
using System.Web.Mvc;

namespace Dlw.EpiBase.Content.Infrastructure.Extensions.SelectMany
{
    /// <summary>
    /// Extension method to split string used by SelectManyAttribute
    /// </summary>
    public static class StringExtensions
    {
        public static string[] SplitSelectMany(this String str)
        {
            return str.Split(',');
        }
        public static IHtmlString ToLineBreakString(this string original)
        {
            var parsed = string.Empty;
            if (string.IsNullOrWhiteSpace(original)) return new MvcHtmlString(parsed);
            parsed = HttpUtility.HtmlEncode(original);
            parsed = parsed.Replace("\n", "<br />");

            return new MvcHtmlString(parsed);
        }

    }
}
