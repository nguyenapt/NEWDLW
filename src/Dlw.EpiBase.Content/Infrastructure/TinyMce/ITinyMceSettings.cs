using EPiServer.Core.PropertySettings;
using System;

namespace Dlw.EpiBase.Content.Infrastructure.TinyMce
{
    public interface ITinyMceSettings
    {
        string DisplayName { get; set; }
        Guid Id { get; set; }
        string ContentCss { get; set; }
        string[][] Toolbars { get; set; }
        string[] NonVisualPlugins { get; set; }
        PropertySettingsContainer GetOrCreateSettingContainer(IPropertySettingsRepository settingsRepository);
    }
}
