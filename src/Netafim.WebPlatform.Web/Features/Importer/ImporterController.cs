using System.Web;
using System.Web.Mvc;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;

namespace Netafim.WebPlatform.Web.Features.Importer
{
    [Authorize(Roles = "Administrators")]
    public class ImporterController : Controller
    {
        private readonly ISystemConfiguratorImporter _systemConfiguratorImporter;
        
        public ImporterController(ISystemConfiguratorImporter systemConfiguratorImporter)
        {
            _systemConfiguratorImporter = systemConfiguratorImporter;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SystemConfigurator(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return RedirectToAction("Index");
            }

            var result = _systemConfiguratorImporter.Import(file.InputStream);

            return Content(result.Success
                ? "<div><h2>Import succeeded</h2></div>"
                : $"<div><h2>Import failed</h2><p>Reason: {result.Message}</p></div>");
        }
    }
}