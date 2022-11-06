using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Dlw.EpiBase.Content.Infrastructure;
using EPiServer.Logging;

namespace Netafim.WebPlatform.Web
{
    public partial class EPiServerApplication : EPiServer.Global
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(EPiServerApplication));

        static EPiServerApplication()
        {
            EnableLocalAppSettings();

            EnsureAppSettingsAsEnvironmentVariables();

            // Disabled file generation, caused recycle. (temp)
            //ConfigureLogging();
        }

        protected void Application_Start()
        {
            ShutdownTracker.Start();

            ConfigureApplicationInsights();

            AreaRegistration.RegisterAllAreas();

            ConfigureViewEngine();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }

        protected void Application_Error()
        {
            // hack to handle exception through httpErrors section and not customErrors
            // sources:
            // https://dusted.codes/demystifying-aspnet-mvc-5-error-pages-and-error-logging
            // https://stackoverflow.com/a/21271085/23805

            var error = Server.GetLastError();
            Server.ClearError();

            if (error != null)
            {
                _logger.Error($"Unhandled error occured: '{error.Message}'.", error);
            }
            // exception also logged by AppInsights

            var httpException = error as HttpException;
            if (Response.StatusCode != 404)
            {
                Response.StatusCode = httpException?.GetHttpCode() ?? (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}