using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.JobDetails;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;
using System.Web.Mvc;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public class JobFilterController : ListingBaseBlockController<JobFilterBlock, JobFilterBlockQueryViewModel, JobDetailsPage>
    {
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly IJobRepository _jobRepo;
        public JobFilterController(IContentLoader contentLoader,
            IPageService pageService,
            IFindSettings findSettings,
            IPageRouteHelper pageRouteHelper,
            IJobRepository jobRepo)
            : base(contentLoader, pageService, findSettings)
        {
            _pageRouteHelper = pageRouteHelper;
            _jobRepo = jobRepo;
        }
        public override ActionResult Index(JobFilterBlock currentContent)
        {
            var model = new JobFilterBlockViewModel(currentContent)
            {
                Departments = _jobRepo.GetAllDepartments(),
                Locations = _jobRepo.GetAllLocations()
            };

            return PartialView(FullViewPath(currentContent), model);
        }
        protected override string ResultViewPath(JobFilterBlock model)
        {
            return string.Format(Core.Templates.Global.Constants.AbsoluteViewPathFormat, "JobFilter", "_searchResult");
        }       
    }
}