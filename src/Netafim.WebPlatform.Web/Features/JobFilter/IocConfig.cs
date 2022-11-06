using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.Settings.Impl;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Find;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    [InitializableModule]
    public class IocConfig : IConfigurableModule
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(IocConfig));

        public void Initialize(InitializationEngine context)
        {
            // do nothing
        }

        public void Uninitialize(InitializationEngine context)
        {
            // do nothing
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IJobRepository, JobRepository>();
            context.Services.AddTransient<IJobFilterSettings, PageJobSettings>();
            context.Services.AddTransient<IQueryComposer, JobFilterQueryComposer>();
            context.Services.AddTransient<IContentGenerator, JobFilterContentGenerator>();
        }
    }
}