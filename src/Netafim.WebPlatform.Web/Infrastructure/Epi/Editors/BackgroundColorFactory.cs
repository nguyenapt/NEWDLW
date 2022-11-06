using DbLocalizationProvider;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Editors
{
    public class BackgroundColorFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new[]
            {
                new SelectItem() { Text = LocalizationProvider.Current.GetString(() => Labels.White), Value = BackgroundColor.White },
                new SelectItem() { Text = LocalizationProvider.Current.GetString(() => Labels.Grey), Value = BackgroundColor.Grey }
            };
        }
    }
    
    /// <summary>
    /// Colors type of the background color
    /// </summary>
    public enum BackgroundColor
    {
        White,
        Grey,
    }
}