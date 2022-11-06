using System.Net.Http;
using System.Threading.Tasks;

namespace Netafim.WebPlatform.Web.Core.Rest
{
    public interface IRestApiOptions
    {
        string BaseAddress { get; }
    }

    public interface ISecuredApiOptions : IRestApiOptions
    {
        string ClientId { get; }
        string ClientSecret { get; }
    }

    public interface IHttpClientInterceptor
    {
        Task Intercept(HttpClient client);

        bool IsSatisfied(IRestService client);
    }
}
