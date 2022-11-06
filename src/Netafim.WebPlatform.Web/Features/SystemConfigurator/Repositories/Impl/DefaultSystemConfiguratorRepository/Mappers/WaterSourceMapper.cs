using System.Collections.Generic;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Mappers
{
    public static class WaterSourceMapper
    {
        public static IEnumerable<WaterSource> Map(this IEnumerable<WaterSourceEntity> from)
        {
            return from.Select(f => f.Map());
        }

        public static WaterSource Map(this WaterSourceEntity from)
        {
            return new WaterSource
            {
                Id = from.Id,
                Name = from.Name
            };
        }
    }
}