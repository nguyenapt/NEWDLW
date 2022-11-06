using System;
using Dlw.EpiBase.Content.Infrastructure;

namespace Dlw.EpiBase.Content.Cms.Search
{
    public class ConfigurationManagerFindSettings : BaseConfigurationManagerSettings, IFindSettings
    {
        public TimeSpan CacheDuration => TimeSpan.FromMinutes(5);

        public int MaxItemsPerRequest => 1000;
    }
}