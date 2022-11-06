using EPiServer.Core.PropertySettings;
using EPiServer.Editor.TinyMCE;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dlw.EpiBase.Content.Infrastructure.TinyMce
{
    public abstract class TinyMceSettingBase
    {
        public string ContentCss { get; set; }

        public string DisplayName { get; set; }

        public Guid Id { get; set; }

        public string[] NonVisualPlugins { get; set; }

        public string[][] Toolbars { get; set; }

        protected virtual int Width { get { return 580; } }

        protected virtual int Height { get { return 300; } }

        public virtual PropertySettingsContainer GetOrCreateSettingContainer(IPropertySettingsRepository settingsRepository)
        {
            PropertySettingsContainer container;
            bool needToSave = false;
            settingsRepository.TryGetContainer(this.Id, out container);


            if (container == null)
                needToSave = true;

            container = container ?? new PropertySettingsContainer(this.Id);

            var wrapper = container.GetSetting(typeof(TinyMCESettings));
            if (wrapper == null)
                needToSave = true;

            wrapper = wrapper ?? new PropertySettingsWrapper();

            var propertySettings = wrapper.PropertySettings as TinyMCESettings;
            if (propertySettings == null)
            {
                propertySettings = new TinyMCESettings();
                needToSave = true;
            }

            needToSave = needToSave || HasPropertySettingsChanged(propertySettings) || HasPropertySettingsWrapperChanged(wrapper);

            UpdateSettings(wrapper, propertySettings);

            container.AddSettings(wrapper);

            if (needToSave)
                settingsRepository.Save(container);

            return container;
        }
        
        protected virtual bool HasPropertySettingsChanged(TinyMCESettings localSettings)
        {
            if (HasNonVisualPluginsChanged(localSettings)
                || HasConentCssChanged(localSettings)
                || localSettings.Width != this.Width
                || localSettings.Height != this.Height)
                return true;

            return HasToolbarsChanged(localSettings);
        }

        private bool HasToolbarsChanged(TinyMCESettings localSettings)
        {
            for (int i = 0; i < localSettings.ToolbarRows.Count; i++)
            {
                if (i >= this.Toolbars.Length)
                    return true;

                var tools = this.Toolbars[i] ?? new string[] { };

                foreach (var t in localSettings.ToolbarRows[i].Buttons)
                {
                    localSettings.ToolbarRows[i].Buttons = localSettings.ToolbarRows[i].Buttons ?? new List<string>();

                    if (!localSettings.ToolbarRows[i].Buttons.SequenceEqual(tools))
                        return true;
                }
            }

            return false;
        }

        private bool HasNonVisualPluginsChanged(TinyMCESettings settings)
        {
            settings.NonVisualPlugins = settings.NonVisualPlugins ?? new string[] { };
            this.NonVisualPlugins = this.NonVisualPlugins ?? new string[] { };

            return !this.NonVisualPlugins.SequenceEqual(settings.NonVisualPlugins);
        }

        private bool HasConentCssChanged(TinyMCESettings settings)
        {
            settings.ContentCss = settings.ContentCss ?? string.Empty;
            this.ContentCss = this.ContentCss ?? string.Empty;

            return !this.ContentCss.Equals(settings.ContentCss);
        }

        private bool HasPropertySettingsWrapperChanged(PropertySettingsWrapper wrapper)
        {
            wrapper.DisplayName = wrapper.DisplayName ?? string.Empty;
            this.DisplayName = this.DisplayName ?? string.Empty;

            return !wrapper.DisplayName.Equals(this.DisplayName);
        }

        private void UpdateSettings(PropertySettingsWrapper wrapper, TinyMCESettings propertySettings)
        {
            propertySettings.NonVisualPlugins = this.NonVisualPlugins ?? new string[] { };
            propertySettings.ContentCss = this.ContentCss ?? string.Empty;
            propertySettings.Width = this.Width;
            propertySettings.Height = this.Height;

            propertySettings.ToolbarRows.Clear();
            foreach (var toolbarRow in this.Toolbars)
            {
                propertySettings.ToolbarRows.Add(new ToolbarRow(toolbarRow));
            }
            wrapper.PropertySettings = propertySettings;
            wrapper.IsGlobal = true;
            wrapper.IsDefault = false;
            wrapper.DisplayName = this.DisplayName;
        }
    }
}
