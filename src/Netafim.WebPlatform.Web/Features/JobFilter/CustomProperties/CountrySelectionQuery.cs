using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties
{
    [ServiceConfiguration(typeof(ISelectionQuery))]
    public class CountrySelectionQuery : ISelectionQuery
    {
        List<SelectItem> _items;
        public CountrySelectionQuery()
        {
            _items = new List<SelectItem>();
            var countryRepository = ServiceLocator.Current.GetInstance<ICountryRepository>();
            _items = countryRepository.GetCountries()
                .OrderBy(t => t.Value)
                .Select(country => new SelectItem
                {
                    Text = countryRepository.GetCountryNameByISOCode(country.Key),
                    Value = country.Key
                }).ToList();
        }
        public ISelectItem GetItemByValue(string value)
        {
            return _items.FirstOrDefault(i => i.Value.Equals(value));
        }

        public IEnumerable<ISelectItem> GetItems(string query)
        {
            return _items.Where(i => i.Text.StartsWith(query, StringComparison.OrdinalIgnoreCase));
        }
    }
}