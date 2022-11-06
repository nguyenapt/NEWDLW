using DbLocalizationProvider;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.ProductFamily.Criteria
{
    public class SeasonFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
           {
                    new SelectItem { Text = LocalizationProvider.Current.GetString(Labels.SingleSeason), Value  = (int)Season.Single },
                    new SelectItem { Text = LocalizationProvider.Current.GetString(Labels.SemiPermanentSeason), Value  = (int)Season.SemiPermanent },
                    new SelectItem { Text = LocalizationProvider.Current.GetString(Labels.PermanentSeason), Value  = (int)Season.Permanent }
           };
        }
    }
    public enum Season
    {
        Single = 3,
        SemiPermanent = 8,
        Permanent = 15,
        MaxValue=15,
    }
}