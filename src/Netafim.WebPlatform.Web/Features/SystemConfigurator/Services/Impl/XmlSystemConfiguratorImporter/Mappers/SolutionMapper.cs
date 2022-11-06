using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class SolutionMapper
    {
        public static IEnumerable<SolutionEntity> Map(this IEnumerable<Solution> from, CultureInfo culture)
        {
            return from.Select(f => Map((Solution) f, culture));
        }

        public static SolutionEntity Map(this Solution from, CultureInfo culture)
        {
            return new SolutionEntity
            {
                Key = from.Key,
                Culture = culture.Name,
                ContentReference = from.ContentReference
            };
        }
    }
}