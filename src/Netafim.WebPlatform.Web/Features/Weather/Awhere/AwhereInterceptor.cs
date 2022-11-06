using EPiServer;
using Netafim.WebPlatform.Web.Core.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Netafim.WebPlatform.Web.Features.Weather.Awhere
{

    public class AwhereInterceptor : IHttpClientInterceptor
    {
        private const string Key = "Access_Token_Of_AwhereToken";

        protected readonly AwhereApiOptions AwhereOptions;

        public AwhereInterceptor(AwhereApiOptions awhereApiOptions)
        {
            this.AwhereOptions = awhereApiOptions;
        }

        public async Task Intercept(HttpClient client)
        {
            var accessToken = CacheManager.Get(Key) as AwhereAccessToken;

            if (accessToken == null || accessToken.Expired <= DateTime.Now)
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                var requestBase = $"{AwhereOptions.ClientId.Trim()}:{AwhereOptions.ClientSecret.Trim()}";
                string encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(requestBase));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedString);
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                var response = client.PostAsync("/oauth/token", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    accessToken = await response.Content.ReadAsAsync<AwhereAccessToken>();
                    accessToken.Expired = DateTime.Now.AddSeconds(accessToken.ExpiredIn);

                    CacheManager.Insert(Key, accessToken);
                }
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        }

        public bool IsSatisfied(IRestService client)
        {
            return client is AwhereService;
        }
    }
}