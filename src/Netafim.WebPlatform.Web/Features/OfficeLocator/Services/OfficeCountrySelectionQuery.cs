using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator.Services
{
    [ServiceConfiguration(typeof(ISelectionQuery))]
    public class OfficeCountrySelectionQuery : ISelectionQuery
    {
        readonly List<SelectItem> _items;
        public OfficeCountrySelectionQuery()
        {
            _items = new List<SelectItem>();
            var countries = new Dictionary<string, string>();
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                      .Select(cultureInfo => new RegionInfo(cultureInfo.Name));

            foreach (var regionInfo in cultures.Where(regionInfo => !countries.ContainsKey(regionInfo.TwoLetterISORegionName)))
            {
                countries.Add(regionInfo.TwoLetterISORegionName, regionInfo.EnglishName);
            }

            _items.AddRange(countries.Select(i => new SelectItem() { Text = i.Value, Value = i.Key }));
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