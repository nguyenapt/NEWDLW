using System.Collections.Generic;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Mappers
{
    public static class CropMapper
    {
        public static IEnumerable<Crop> Map(this IEnumerable<CropEntity> from)
        {
            return from.Select(f => f.Map());
        }

        public static Crop Map(this CropEntity from)
        {
            return new Crop
            {
                Id = from.Id,
                Name = from.Name
            };
        }
    }
}