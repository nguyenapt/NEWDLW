using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class ConnectorMapper
    {
        public static IEnumerable<ConnectorEntity> Map(this IEnumerable<Connector> from, CultureInfo culture)
        {
            return from.Select(f => Map(f, culture));
        }

        public static ConnectorEntity Map(this Connector from, CultureInfo culture)
        {
            return new ConnectorEntity
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