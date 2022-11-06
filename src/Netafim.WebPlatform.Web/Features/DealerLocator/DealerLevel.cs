using DbLocalizationProvider;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.ComponentModel;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    public enum DealerLevel
    {
        [Description(DealerConstants.PartnerDescription)]
        Partner = 1,

        [Description(DealerConstants.SecondaryDescription)]
        Secondary = 2,

        [Description(DealerConstants.ThirdaryDescription)]
        Thirdary = 4
    }

    public enum DealerColor
    {
        [Description(DealerConstants.ColourBlue)]
        Blue,

        [Description(DealerConstants.ColourOrange)]
        Orange,

        [Description(DealerConstants.ColourDarkBlue)]
        DarkBlue,
    }

    public class DealerLevelFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new[]
            {
                new SelectItem() { Text = LocalizationProvider.Current.GetString(() => Labels.Partner), Value = DealerLevel.Partner },
                new SelectItem() { Text = LocalizationProvider.Current.GetString(() => Labels.Secondary), Value = DealerLevel.Secondary },
                new SelectItem() { Text = LocalizationProvider.Current.GetString(() => Labels.Thirdary), Value = DealerLevel.Thirdary },
            };
        }
    }

    public class DealerColorFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new[]
            {
                new SelectItem() { Text = LocalizationProvider.Current.GetString(() => Labels.Blue), Value = DealerColor.Blue },
                new SelectItem() { Text = LocalizationProvider.Current.GetString(() => Labels.Orange), Value = DealerColor.Orange },
                new SelectItem() { Text = LocalizationProvider.Current.GetString(() => Labels.DarkBlue), Value = DealerColor.DarkBlue },
            };
        }
    }
}