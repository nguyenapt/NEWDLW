using Microsoft.Owin;

namespace Netafim.WebPlatform.Web.Infrastructure.Owin
{
    public static class OwinRequestExtensions
    {
        internal static class CommonKeys
        {
            public const string IsLocal = "server.IsLocal";
        }

        public static bool IsLocal(this IOwinRequest request)
        {
            return request.Get<bool>(CommonKeys.IsLocal);
        }
    }
}