using EPiServer.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class IpAddessExtensions
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        public static IPAddress ConvertToIpAddress(this string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return null;
            }

            // Ex: 103.248.164.14:34968
            if (ipAddress.Contains(":"))
            {
                // 103.248.164.14:34968 -> 103.248.164.14
                ipAddress = ipAddress.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            IPAddress result;

            try
            {
                result = IPAddress.Parse(ipAddress);
            }

            catch
            {
                result = IPAddress.None;
            }

            return result;
        }

        public static IPAddress GetClientFullIpAddress(this HttpRequestBase request)
        {
            return request.GetClientIpAddress().ConvertToIpAddress();
        }

        public static string GetClientIpAddress(this HttpRequestBase request)
        {
            string userIp = null;
            userIp = !string.IsNullOrEmpty(request.QueryString["ipaddress"])
                ? request.QueryString["ipaddress"]
                : GetUserIPAddress(request);

            _logger.Debug(string.Format("ipaddress: {0}", userIp));

            return userIp;
        }
        
        private static string GetUserIPAddress(HttpRequestBase request)
        {
#if DEBUG
            return ConfigurationManager.AppSettings["geo:iptest"]; //stockholm
#else

            _logger.Debug(string.Format("Request.UserHostAddress: {0}", request.UserHostAddress));

            var ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            _logger.Debug(string.Format("HTTP_X_FORWARDED_FOR: {0}", ipList));
            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            var remoteAddress = request.ServerVariables["REMOTE_ADDR"];
            _logger.Debug(string.Format("REMOTE_ADDR: {0}", remoteAddress));
            return remoteAddress;
#endif
        }
    }
}