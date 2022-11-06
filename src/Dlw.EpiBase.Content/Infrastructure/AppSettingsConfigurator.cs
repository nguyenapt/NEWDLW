using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using EPiServer.Logging;

namespace Dlw.EpiBase.Content.Infrastructure
{
    public class AppSettingsConfigurator
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(AppSettingsConfigurator));

        public void LoadAppSettings(string relativePath)
        {
            var fullPath = GetFullPath(relativePath);

            if (!File.Exists(fullPath))
            {
                _logger.Debug($"File with absolute path '{fullPath}' not found.");
                return;
            }

            _logger.Information("Custom AppSettings file found.");

            var appSettings = XDocument.Load(fullPath);
            var pairs = (from setting in appSettings.Descendants("add") select setting)
                .ToDictionary(key => (string) key.Attribute("key"), value => (string) value.Attribute("value"));

            foreach (var pair in pairs)
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains(pair.Key))
                {
                    throw new Exception($"No appsetting found with key '{pair.Key}'.");
                }

                var previousValue = ConfigurationManager.AppSettings[pair.Key];

                ConfigurationManager.AppSettings[pair.Key] = pair.Value;

                _logger.Information($"AppSettings '{pair.Key}' changed from '{previousValue}' to '{ConfigurationManager.AppSettings[pair.Key]}'.");
            }
        }

        private string GetFullPath(string relativePath)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(baseDirectory, relativePath);
        }
    }
}