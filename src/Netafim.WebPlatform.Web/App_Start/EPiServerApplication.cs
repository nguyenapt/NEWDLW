using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Dlw.EpiBase.Content.Infrastructure;
using Dlw.EpiBase.Content.Infrastructure.Mvc;
using Microsoft.ApplicationInsights.Extensibility;

namespace Netafim.WebPlatform.Web
{
    public partial class EPiServerApplication
    {
        protected override void RegisterRoutes(RouteCollection routes)
        {
            base.RegisterRoutes(routes);

            //Very quick fix to get rid of exception - should do further like this: http://jondjones.com/episerver-7-routing-for-dummies/
            routes.MapRoute("defaultwithculture", "{language}/{controller}/{action}", new { action = "index" },
                new { language = @"[a-zA-Z]{2}(-[a-zA-Z]{2}$|$)" });

            // be able to use default mvc routes

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional });

        }

        private static void ConfigureLogging()
        {
            var configurator = new Log4NetEnvironmentVariablesConfigurator();

            configurator.ReplaceEnvironmentVariables("log4net.config", "EPiServerLog.config");
        }

        private static void EnsureAppSettingsAsEnvironmentVariables()
        {
            var configurator = new EnvironmentVariablesConfigurator();
            configurator.EnsureAppSettings();
        }

        private void ConfigureViewEngine()
        {
            var viewEngineConfig = new ViewEngineConfigurator();
            viewEngineConfig.Configure(ViewEngines.Engines);
        }

        private static void EnableLocalAppSettings()
        {
            var configurator = new AppSettingsConfigurator();
            configurator.LoadAppSettings("App_Config/AppSettings.local.config");
        }

        private void ConfigureApplicationInsights()
        {
            var instrumentationKey = ConfigurationManager.AppSettings["ApplicationInsights.InstrumentationKey"];
            if (string.IsNullOrEmpty(instrumentationKey))
            {
                TelemetryConfiguration.Active.DisableTelemetry = true;
                return;
            }

            TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;
        }
    }
}