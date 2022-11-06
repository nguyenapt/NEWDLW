using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties
{
    [ServiceConfiguration(typeof(ISelectionQuery))]
    public class JobPositionSelectionQuery : ISelectionQuery
    {
        List<SelectItem> _items;
        public JobPositionSelectionQuery()
        {
            _items = new List<SelectItem>();
            var jobRepo = ServiceLocator.Current.GetInstance<IJobRepository>();
            _items = jobRepo.GetAllPositions()
                .OrderBy(t => t.Key)
                .Select(department => new SelectItem
                {
                    Text = department.Value,
                    Value = department.Key
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