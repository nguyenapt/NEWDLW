using System;
using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;
using EPiServer.ServiceLocation;
using EPiServer.DataAbstraction;
using Castle.Core.Internal;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{

    [ServiceConfiguration(typeof(ISelectionQuery))]
    public class LanguageLinkSelectionQuery : ISelectionQuery
    {
        List<SelectItem> _items;
        public LanguageLinkSelectionQuery()
        {
            var allLangs = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>().ListAll().Where(x => x.IsLocalSite());
            _items = new List<SelectItem>();
            if (allLangs.IsNullOrEmpty()) { return; }
            foreach (var item in allLangs)
            {
                var culture = item.Culture;
                if (culture == null) continue;
                _items.Add(new SelectItem()
                {
                    Text = culture.EnglishName,
                    Value = culture.Name
                });
            }
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