using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class ContentRelationMapper
    {
        public static IEnumerable<DecisionTreeEntity> Map(this IEnumerable<ContentRelation> from, IEnumerable<ContentEntity> content, IEnumerable<CropEntity> crops, IEnumerable<RegionEntity> regions, CultureInfo culture)
        {
            return from.Select(f => Map(f, content, crops, regions, culture));
        }

        public static DecisionTreeEntity Map(this ContentRelation from, IEnumerable<ContentEntity> content, IEnumerable<CropEntity> crops, IEnumerable<RegionEntity> regions, CultureInfo culture)
        {
            return new DecisionTreeEntity
            {
                Crop = crops.SingleOrDefault(c => c.Key == from.CropKey),
                Region = regions.SingleOrDefault(c => c.Key == from.RegionKey),
                Entity = content.SingleOrDefault(c => c.Key == from.ContentKey),
                Culture = culture.Name
            };
        }
    }
}