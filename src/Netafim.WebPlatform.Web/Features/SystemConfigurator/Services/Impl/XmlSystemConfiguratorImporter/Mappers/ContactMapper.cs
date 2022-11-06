using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Mappers
{
    public static class ContactMapper
    {
        public static IEnumerable<ContactEntity> Map(this IEnumerable<Contact> from, CultureInfo culture)
        {
            return from.Select(f => Map(f, culture));
        }

        public static ContactEntity Map(this Contact from, CultureInfo culture)
        {
            return new ContactEntity
            {
                Key = from.Key,
                Email = from.Email,
                PhoneNumber = from.PhoneNumber,
                FirstName = from.FirstName,
                LastName = from.LastName,
                Title = from.Title,
                Culture = culture.Name
            };
        }
    }
}