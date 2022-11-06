using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class RegionMapper
    {
        public static IEnumerable<RegionEntity> Map(this IEnumerable<Region> from, CultureInfo culture)
        {
            return from.Select(f => f.Map(culture));
        }

        public static RegionEntity Map(this Region from, CultureInfo culture)
        {
            return new RegionEntity
            {
                Key = from.Key,
                Name = from.Name,
                Eto = from.Eto,
                Dealer = from.Dealer,
                DealerPhone = from.DealerPhone,
                NetafimSales = from.NetafimSales,
                NetafimPhone = from.NetafimPhone,
                Culture = culture.Name
            };
        }
    }
}