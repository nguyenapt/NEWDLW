using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using Netafim.WebPlatform.Web.Core.Repository;

namespace Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties
{
    [ServiceConfiguration(typeof(ISelectionQuery))]
    public class JobLocationSelectionQuery : ISelectionQuery
    {
        List<SelectItem> _items;
        public JobLocationSelectionQuery()
        {
            _items = new List<SelectItem>();
            var jobRepository = ServiceLocator.Current.GetInstance<IJobRepository>();
            var countryRepository = ServiceLocator.Current.GetInstance<ICountryRepository>();
            _items = jobRepository.GetAllLocations()
                .OrderBy(t => t.Key)
                .Select(location => new SelectItem
                {
                    Text = countryRepository.GetCountryNameByISOCode(location.Key),
                    Value = location.Key
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