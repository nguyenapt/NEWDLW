using System.Collections.Generic;
using System.Linq;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Core.Repository;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Editors
{
    public class CountrySelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var countryRepository = ServiceLocator.Current.GetInstance<ICountryRepository>();
            return countryRepository.GetCountries()
                .OrderBy(t => t.Value)
                .Select(country => new SelectItem
                {
                    Text = country.Value,
                    Value = country.Key
                });
        }
    }
}