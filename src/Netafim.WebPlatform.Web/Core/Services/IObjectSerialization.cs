using Newtonsoft.Json;

namespace Netafim.WebPlatform.Web.Core.Services
{
    public interface IObjectSerialization
    {
        string ToJson<TEntity>(TEntity source);
    }
}
