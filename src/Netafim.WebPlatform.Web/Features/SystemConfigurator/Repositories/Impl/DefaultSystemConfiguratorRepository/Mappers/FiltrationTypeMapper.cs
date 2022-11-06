using System.Collections.Generic;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Mappers
{
    public static class FiltrationTypeMapper
    {
        public static IEnumerable<FiltrationType> Map(this IEnumerable<FiltrationTypeEntity> from)
        {
            return from.Select(f => f.Map());
        }

        public static FiltrationType Map(this FiltrationTypeEntity from)
        {
            return new FiltrationType
            {
                Id = from.Id,
                Name = from.Name
            };
        }
    }
}