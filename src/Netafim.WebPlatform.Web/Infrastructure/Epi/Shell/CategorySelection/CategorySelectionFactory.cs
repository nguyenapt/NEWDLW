using System.Collections.Generic;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.CategorySelection
{
    public class CategorySelectionFactory : ISelectionFactory
    {
        protected CategoryRepository Repository { get; }

        protected EPiServer.DataAbstraction.Category StartCategory { get; set; }

        public CategorySelectionFactory()
        {
            Repository = ServiceLocator.Current.GetInstance<CategoryRepository>();

            GetStartNode();
        }

        protected virtual void GetStartNode()
        {
            StartCategory = Repository.GetRoot();
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var list = new List<ISelectItem>();

            if (StartCategory != null)
            {
                AddItems(StartCategory, list);
            }

            return list.ToArray();
        }

        private void AddItems(EPiServer.DataAbstraction.Category category, ICollection<ISelectItem> items)
        {
            foreach (var cat in category.Categories)
            {
                if (cat.Selectable)
                {
                    var prefix = GetParentPath(category);
                    items.Add(new SelectItem() { Text = $"{prefix}{cat.Name}", Value = cat.ID });
                }

                AddItems(cat, items);
            }
        }

        private string GetParentPath(EPiServer.DataAbstraction.Category category)
        {
            string prefix = string.Empty;

            if (category.Parent == null) return prefix;

            while (category.Parent != null)
            {
                prefix = $"{category.Name}->{prefix}";

                category = category.Parent;
            }

            return prefix;
        }
    }

    public class CategorySelectionFactory<T> : CategorySelectionFactory where T : Category
    {
        protected override void GetStartNode()
        {
            StartCategory = Repository.Get(typeof(T).Name);
        }
    }
}