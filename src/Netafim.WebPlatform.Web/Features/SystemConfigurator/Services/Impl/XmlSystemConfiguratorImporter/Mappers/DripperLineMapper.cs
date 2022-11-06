using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class DripperLineMapper
    {
        public static IEnumerable<DripperLineEntity> Map(this IEnumerable<DripperLine> from, CultureInfo culture)
        {
            return from.Select(f => Map(f, culture));
        }

        public static DripperLineEntity Map(this DripperLine from, CultureInfo culture)
        {
            return new DripperLineEntity
            {
                Key = from.Key,
                CatalogNumber = from.CatalogNumber,
                Name = from.Name,
                FlowRate = from.FlowRate,
                EmiterSpacing = from.EmiterSpacing,
                NumberOfLaterals = from.NumberOfLaterals,
                FlowVariation = from.FlowVariation,
                ContentReference = from.ContentReference,
                Culture = culture.Name
            };
        }
    }
}