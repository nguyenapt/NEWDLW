using System;
using System.Configuration;
using System.Globalization;

namespace Dlw.EpiBase.Content.Infrastructure
{
    public class BaseConfigurationManagerSettings
    {
        protected T GetAppSetting<T>(string key, bool required = true)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                if (required) throw new Exception($"AppSetting '{key}' is required but not set.");

                return default(T);
            }

            return (T) Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}