using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core;
using EPiServer.Forms.Core.Models;
using EPiServer.Forms.Core.Models.Internal;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    public static class EmailBuilder
    {
        public static string BuildHtmlContent(this IEnumerable<FriendlyNameInfo> formElements, Submission submission)
        {
            var content = "";

            if (!formElements.Any())
                return content;

            foreach (var element in formElements.Where(t => t.FormatType != FormatType.Link))
            {
                var value = submission.GetElementValueFromFriendlyName(formElements, element.FriendlyName);
                if(string.IsNullOrWhiteSpace(value))
                    continue;
                var label = string.IsNullOrEmpty(element.Label) ? element.FriendlyName : element.Label;
                content += GenerateHtmlContent(label, value);
            }

            return content;
        }

        public static IEnumerable<string> GetFileUploadUrls(this IEnumerable<FriendlyNameInfo> formElements, Submission submission)
        {
            var fieldUploads = formElements.Where(f => f.FormatType == FormatType.Link);
            return !fieldUploads.Any() ? new List<string>() : fieldUploads.Select(element => submission.GetElementValueFromFriendlyName(fieldUploads, element.FriendlyName));
        }

        public static string GetElementValueFromFriendlyName(this Submission submission, IEnumerable<FriendlyNameInfo> friendlyNameInfo, string friendlyName)
        {
            var friendlyPageId = friendlyNameInfo.FirstOrDefault(x => string.Equals(x.FriendlyName, friendlyName, StringComparison.CurrentCultureIgnoreCase));
            if (string.IsNullOrWhiteSpace(friendlyPageId?.ElementId))
                return string.Empty;

            return submission.Data.ContainsKey(friendlyPageId.ElementId) ? submission.Data[friendlyPageId.ElementId].ToString() : string.Empty;
        }

        public static byte[] GetFileUploadDataFromUrl(this string url)
        {
            var urlBuilder = new UrlBuilder(url);
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var content = urlResolver.Route(urlBuilder);
            if (content != null)
            {
                var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
                var mediaData = contentRepository.Get<MediaData>(content?.ContentLink);
                return GetFileUploadDataFromMediaData(mediaData);
            }

            var absoluteUri = UriSupport.CreateAbsoluteUri(new Url(url));

            return GetFileUploadDataFromUri(absoluteUri.AbsoluteUri);
        }

        public static IEnumerable<FriendlyNameInfo> GetFriendlyNameInfos(this FormIdentity formIdentity)
        {
            var formRepository = ServiceLocator.Current.GetInstance<IFormRepository>();
            return formRepository.GetFriendlyNameInfos(formIdentity, typeof (IExcludeInSubmission))
                .Where(t => !t.FriendlyName.StartsWith("SYSTEMCOLUMN"));
        }

        private static byte[] GetFileUploadDataFromMediaData(MediaData data)
        {
            using (var blob = data.BinaryData.OpenRead())
            {
                var ms = new MemoryStream();
                blob.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);

                return ms.ToArray();
            }
        }

        private static byte[] GetFileUploadDataFromUri(string absoluteUri)
        {
            var req = (HttpWebRequest)WebRequest.Create(absoluteUri);
            using (var httpWResp = (HttpWebResponse)req.GetResponse())
            {
                using (var responseStream = httpWResp.GetResponseStream())
                {
                    var ms = new MemoryStream();
                    responseStream?.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    return ms.ToArray();
                }
            }
        }

        private static string GenerateHtmlContent(string label, string value)
        {
            var htmlValue = $"<td style='-moz-hyphens:auto;-webkit-hyphens:auto;Margin:0;border-collapse:collapse!important;color:#666;font-family:Roboto,Helvetica,Arial,sans-serif;font-size:15px;font-weight:400;hyphens:auto;line-height:1.3;margin:0;padding:0;text-align:left;vertical-align:top;word-wrap:break-word'><span>{value}</span></td>";

            var content = $"<tr style='padding:0;text-align:left;vertical-align:top'><td width='40%' style='-moz-hyphens:auto;-webkit-hyphens:auto;Margin:0;border-collapse:collapse!important;color:#666;font-family:Roboto,Helvetica,Arial,sans-serif;font-size:15px;font-weight:400;hyphens:auto;line-height:1.3;margin:0;padding:0;padding-right:20px;text-align:right;vertical-align:top;word-wrap:break-word'><span style='font-weight:700'>{label}</span></td>{htmlValue}</tr>";

            return content;
        }
    }
}