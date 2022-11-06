using System;
using System.Globalization;
using System.Net;

namespace Dlw.EpiBase.Content.Infrastructure.Net
{
    // source: https://stackoverflow.com/questions/2727609/best-way-to-create-ipendpoint-from-string/35357209
    public static class IPEndpointParser
    {
        // Handles IPv4 and IPv6 notation.
        public static IPEndPoint Parse(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (ep.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            int port;
            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }
    }
}