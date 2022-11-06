using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;
using Crop = Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain.Crop;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class WaterSourceMapper
    {
        public static IEnumerable<WaterSourceEntity> Map(this IEnumerable<WaterSource> from, CultureInfo culture)
        {
            return from.Select(f => f.Map(culture));
        }

        public static WaterSourceEntity Map(this WaterSource from, CultureInfo culture)
        {
            return new WaterSourceEntity
            {
                Key = from.Key,
                Name = from.Name,
                Culture = culture.Name
            };
        }
    }
}