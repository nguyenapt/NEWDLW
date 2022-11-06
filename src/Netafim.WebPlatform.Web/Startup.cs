using EPiServer.Logging;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Netafim.WebPlatform.Web.Startup))]
namespace Netafim.WebPlatform.Web
{
    public partial class Startup
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(Startup));

        public void Configuration(IAppBuilder app)
        {
            _logger.Information($"Owin initialization started ({nameof(Startup)}.{nameof(Configuration)}).");

            ConfigureSecurityShield(app);
        }
    }
}