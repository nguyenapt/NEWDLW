using Castle.Core.Internal;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public static class HttpUtils
    {
        public static IRestResponse MakeRequest(string url, Dictionary<string, object> parameters, Method method = Method.GET, Dictionary<string, string> headers = null)
        {
            var client = new RestClient(url);
            var request = new RestRequest(method);
            if (!parameters.IsNullOrEmpty())
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }
            if (!headers.IsNullOrEmpty())
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }
            return client.Execute(request);
        }
    }
}