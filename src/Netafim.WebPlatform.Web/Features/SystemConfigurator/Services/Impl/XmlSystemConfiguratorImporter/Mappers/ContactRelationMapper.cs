using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class ContactRelationMapper
    {
        public static IEnumerable<DecisionTreeEntity> Map(this IEnumerable<ContactRelation> from, IEnumerable<ContactEntity> contact, IEnumerable<CropEntity> crops, IEnumerable<RegionEntity> regions, CultureInfo culture)
        {
            return from.Select(f => Map((ContactRelation) f, contact, crops, regions, culture));
        }

        public static DecisionTreeEntity Map(this ContactRelation from, IEnumerable<ContactEntity> contact, IEnumerable<CropEntity> crops, IEnumerable<RegionEntity> regions, CultureInfo culture)
        {
            return new DecisionTreeEntity
            {
                Crop = crops.SingleOrDefault(c => c.Key == from.CropKey),
                Region = regions.SingleOrDefault(c => c.Key == from.RegionKey),
                Entity = contact.SingleOrDefault(c => c.Key == from.ContactKey),
                Culture = culture.Name
            };
        }
    }
}