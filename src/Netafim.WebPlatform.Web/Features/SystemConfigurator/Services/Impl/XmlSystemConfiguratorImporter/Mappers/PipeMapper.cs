using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class PipeMapper
    {
        public static IEnumerable<PipeEntity> Map(this IEnumerable<Pipe> from, CultureInfo culture)
        {
            return from.Select(f => Map(f, culture));
        }

        public static PipeEntity Map(this Pipe from, CultureInfo culture)
        {
            return new PipeEntity
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