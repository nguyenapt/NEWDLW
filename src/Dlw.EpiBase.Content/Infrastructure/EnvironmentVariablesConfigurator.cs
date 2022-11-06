using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Dlw.EpiBase.Content.Infrastructure
{
    /// <summary>
    /// Utility class to ensure environment variables
    /// </summary>
    public class EnvironmentVariablesConfigurator
    {
        private static readonly Regex Regex = new Regex("[^a-zA-Z0-9 -]");

        public const string AppSettingPrefix = "APP_SETTING";
        public const string SpecialCharacterReplacement = "_";

        public void EnsureAppSettings()
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                var environmentVariableKey = GetKey(key);

                if (Environment.GetEnvironmentVariable(environmentVariableKey) == null)
                {
                    Environment.SetEnvironmentVariable(environmentVariableKey, ConfigurationManager.AppSettings[key]);
                }
            }
        }

        private string GetKey(string key)
        {
            var formattedKey = Regex.Replace(key, SpecialCharacterReplacement).ToUpperInvariant();
            return $"{AppSettingPrefix}_{formattedKey}";
        }
    }
}