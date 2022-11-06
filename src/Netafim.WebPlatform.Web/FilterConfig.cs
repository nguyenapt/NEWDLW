using System.Web.Mvc;
using Netafim.WebPlatform.Web.Infrastructure.Mvc;

namespace Netafim.WebPlatform.Web{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}