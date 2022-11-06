using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Netafim.WebPlatform.Web.Core.Services
{

    public class JsonNetObjectSerialization : IObjectSerialization
    {
        public string ToJson<TEntity>(TEntity source)
        {
            return JsonConvert.SerializeObject(source, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
