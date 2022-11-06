using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class DigitalFarmingMapper
    {
        public static IEnumerable<DigitalFarmingEntity> Map(this IEnumerable<DigitalFarming> from, CultureInfo culture)
        {
            return from.Select(f => Map(f, culture));
        }

        public static DigitalFarmingEntity Map(this DigitalFarming from, CultureInfo culture)
        {
            return new DigitalFarmingEntity
            {
                Key = from.Key,
                Culture = culture.Name,
                ContentReference = from.ContentReference
            };
        }
    }
}