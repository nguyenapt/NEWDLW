using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;

namespace Netafim.WebPlatform.Web.Features.JobDetails
{
    [ContentOutputCache]
    public class JobDetailsController : BasePageController<JobDetailsPage>
    {
        public ActionResult Index(JobDetailsPage currentPage)
        {
            return View(currentPage);
        }
    }
}