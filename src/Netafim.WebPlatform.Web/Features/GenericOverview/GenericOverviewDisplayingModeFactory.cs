using DbLocalizationProvider;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    public class GenericOverviewDisplayingModeFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
           {
                    new SelectItem { Text = LocalizationProvider.Current.GetString(() => Labels.Carousel), Value =GenericOverviewDisplayingMode.Carousel },
                    new SelectItem { Text = LocalizationProvider.Current.GetString(() => Labels.Grid), Value =GenericOverviewDisplayingMode.Grid }
           };
        }
    }
    public enum GenericOverviewDisplayingMode
    {
        Carousel = 1,
        Grid = 2
    }
}