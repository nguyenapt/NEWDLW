using System;

namespace Dlw.EpiBase.Content.Infrastructure.TinyMce
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TinyMceSettingsAttribute : Attribute
    {
        public TinyMceSettingsAttribute(Type settingsType)
        {
            SettingsType = settingsType;
        }

        /// <summary>
        /// Type of setting service
        /// <see cref="ITinyMceSettings"/>
        /// </summary>
        public Type SettingsType { get; set; }
    }
}
