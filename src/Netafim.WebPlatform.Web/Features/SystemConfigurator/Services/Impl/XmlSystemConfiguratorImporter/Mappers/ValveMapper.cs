using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class ValveMapper
    {
        public static IEnumerable<ValveEntity> Map(this IEnumerable<Valve> from, CultureInfo culture)
        {
            return from.Select(f => Map(f, culture));
        }

        public static ValveEntity Map(this Valve from, CultureInfo culture)
        {
            return new ValveEntity
            {
                Key = from.Key,
                CatalogNumber = from.CatalogNumber,
                Name = from.Name,
                ContentReference = from.ContentReference,
                Culture = culture.Name
            };
        }
    }
}