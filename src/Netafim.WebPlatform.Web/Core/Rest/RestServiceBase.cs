using EPiServer.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Netafim.WebPlatform.Web.Core.Rest
{
    public abstract class RestServiceBase<TRestApiOptions> : IRestService
        where TRestApiOptions : IRestApiOptions
    {
        protected readonly HttpClient Client;
        protected readonly TRestApiOptions RestOptions;
        protected readonly ILogger Logger;
        protected readonly IEnumerable<IHttpClientInterceptor> RestInterceptors;

        protected RestServiceBase(HttpClient client,
            TRestApiOptions restApiOptions,
            IEnumerable<IHttpClientInterceptor> restInterceptors)
        {
            this.RestOptions = restApiOptions;
            this.Client = client;
            this.Logger = LogManager.GetLogger();
            this.RestInterceptors = restInterceptors;
        }

        protected virtual async Task<string> GetResultAsStringAsync(string uri)
        {
            var response = this.Client.GetAsync(uri).Result;

            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Log exception
                this.Logger.Error("Exception when calling the Rest api", ex);
                throw new RestException($"Error when get the information with uri {uri} via the REST api", ex);
            }

        }

        protected virtual async Task<TResult> GetResultAsync<TResult>(string uri)
        {
            var jsonResult = await GetResultAsStringAsync(uri);
            return !string.IsNullOrEmpty(jsonResult) ? JsonConvert.DeserializeObject<TResult>(jsonResult) : default(TResult);
        }

        protected virtual async Task<TResult> PostAsync<TResult>(string uri, object value)
        {
            this.Client.DefaultRequestHeaders.Accept.Clear();

            try
            {
                var response = this.Client.PostAsJsonAsync(uri, value).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default(TResult);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<TResult>();
            }
            catch (Exception ex)
            {
                this.Logger.Error($"Exception when post the api {uri}", ex);

                throw new RestException($"Exception when post the api {uri}", ex);
            }
        }

        protected virtual async Task Intercept()
        {
            foreach (var interceptor in this.RestInterceptors.Where(i => i.IsSatisfied(this)))
            {
                await interceptor.Intercept(Client);
            }
        }
    }
}