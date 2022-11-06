using System;
using System.Net;
using System.Web.Mvc;
using EPiServer.Editor;
using EPiServer.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.Error
{
    public class NotFoundController : PageController<NotFoundPage>
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!PageEditing.PageIsInEditMode)
            {
                Response.Status = "404 not found";
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                
                // NEXT, review impact
                //Response.TrySkipIisCustomErrors = true;
            }
        }

        public ActionResult Index(NotFoundPage currentPage)
        {
            if (currentPage == null) throw new ArgumentNullException(nameof(currentPage));

            var model = new NotFoundViewModel()
            {
                NotFoundPage = currentPage
            };

            return View("~/Features/Error/Views/404.cshtml", model);
        }

        public ActionResult Default()
        {
            return File("/404.html", "text/html");
        }
    }
}