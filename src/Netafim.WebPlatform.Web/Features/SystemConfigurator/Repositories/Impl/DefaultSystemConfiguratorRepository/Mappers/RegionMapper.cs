using System.Collections.Generic;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Mappers
{
    public static class RegionMapper
    {
        public static IEnumerable<Region> Map(this IEnumerable<RegionEntity> from)
        {
            return from.Select(f => f.Map());
        }

        public static Region Map(this RegionEntity from)
        {
            return new Region
            {
                Id = from.Id,
                Name = from.Name
            };
        }
    }
}