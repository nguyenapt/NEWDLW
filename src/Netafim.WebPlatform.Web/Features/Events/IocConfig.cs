using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Features.Events
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
            context.Services.AddTransient<IContentGenerator, EventsGenerator>();
            context.Services.AddTransient<IEventsReopsitory, EventsRepository>();
            context.Services.AddTransient<IEventTimeRangeDisplay, EventInDayDisplay>();
            context.Services.AddTransient<IEventTimeRangeDisplay, EventInMonthDisplay>();
            context.Services.AddTransient<IEventTimeRangeDisplay, EventInYearDisplay>();
            context.Services.AddTransient<IEventTimeRangeDisplay, EventInMultipYearsDisplay>();
            context.Services.AddTransient<IEventTimeRangeDisplay, InvalidEventTimeRangDisplay>();
        }
    }
}