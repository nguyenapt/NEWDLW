using EPiServer.Web.Mvc;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell
{
    public abstract class BasePageController<T> : PageController<T> where T : Core.Templates.PageBase
    {
    }
}