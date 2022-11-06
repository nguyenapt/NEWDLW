using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class FiltrationTypeMapper
    {
        public static IEnumerable<FiltrationTypeEntity> Map(this IEnumerable<FiltrationType> from, CultureInfo culture)
        {
            return from.Select(f => f.Map(culture));
        }

        public static FiltrationTypeEntity Map(this FiltrationType from, CultureInfo culture)
        {
            return new FiltrationTypeEntity
            {
                Key = from.Key,
                Name = from.Name,
                Culture = culture.Name
            };
        }
    }
}