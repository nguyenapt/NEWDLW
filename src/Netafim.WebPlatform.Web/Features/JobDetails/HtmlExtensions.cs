using System;
using System.Text;
using System.Web.Mvc;
using EPiServer.Globalization;

namespace Netafim.WebPlatform.Web.Features.JobDetails
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString RenderJobInformation(this HtmlHelper helper, string label, DateTime jobInfo)
        {
            if (jobInfo == DateTime.MinValue)
                return MvcHtmlString.Empty;

            var date = $"{jobInfo.ToString("MMM", ContentLanguage.PreferredCulture)} {jobInfo.ToString("dd")} {jobInfo.ToString("yyyy")}";

            return RenderJobInformation(helper, label, date);
        }

        public static MvcHtmlString RenderJobInformation(this HtmlHelper helper, string label, string jobInfo)
        {
            if (string.IsNullOrEmpty(jobInfo))
                return MvcHtmlString.Empty;

            var liTag = new StringBuilder($"<li><strong>{label}</strong><span>{jobInfo}</span></li>");
            return MvcHtmlString.Create(liTag.ToString());
        }
    }
}