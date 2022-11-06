using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class CropMapper
    {
        public static IEnumerable<CropEntity> Map(this IEnumerable<Crop> from, CultureInfo culture)
        {
            return from.Select(f => f.Map(culture));
        }

        public static CropEntity Map(this Crop from, CultureInfo culture)
        {
            // TODO merge into 1 products collection

            return new CropEntity
            {
                Key = from.Key,
                Group = from.Group,
                Name = from.Name,
                CropFactor = from.CropFactor,
                Culture = culture.Name
            };
        }
    }
}