using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class DealerMapper
    {
        public static IEnumerable<DealerEntity> Map(this IEnumerable<Dealer> from, CultureInfo culture)
        {
            return from.Select(f => Map((Dealer) f, culture));
        }

        public static DealerEntity Map(this Dealer from, CultureInfo culture)
        {
            return new DealerEntity
            {
                Key = from.Key,
                Culture = culture.Name,
                ContentReference = from.ContentReference
            };
        }
    }
}