using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class FiltrationMapper
    {
        public static IEnumerable<FiltrationEntity> Map(this IEnumerable<Filtration> from, IEnumerable<FiltrationTypeEntity> filtrationTypes, IEnumerable<WaterSourceEntity> waterSources, CultureInfo culture)
        {
            return from.Select(f => f.Map(filtrationTypes, waterSources, culture));
        }

        public static FiltrationEntity Map(this Filtration from, IEnumerable<FiltrationTypeEntity> filtrationTypes, IEnumerable<WaterSourceEntity> waterSources, CultureInfo culture)
        {
            return new FiltrationEntity
            {
                Key = from.Key,
                CatalogNumber = from.CatalogNumber,
                Name = from.Name,
                ContentReference = from.ContentReference,
                Culture = culture.Name,
                FlowRate = from.FlowRate,
                TypeOfFiltration = from.TypeOfFiltration,
                FiltrationType = filtrationTypes.FirstOrDefault(f => f.Key.Equals(from.FiltrationType)),
                WaterSource = waterSources.FirstOrDefault(f => f.Key.Equals(from.WaterSource)),
                FamilyName = from.FamilyName
            };
        }
    }
}