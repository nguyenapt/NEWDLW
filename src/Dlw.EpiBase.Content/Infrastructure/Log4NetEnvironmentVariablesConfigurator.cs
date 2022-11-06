using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Dlw.EpiBase.Content.Infrastructure
{
    /// <summary>
    /// Replaces only level attributes.
    //  TODO review support to swap all environment variables? Also these which are natively supported by log4net.
    /// </summary>
    public class Log4NetEnvironmentVariablesConfigurator
    {
        private static readonly Regex Regex = new Regex(@"\${(?<key>.*)}");

        public void ReplaceEnvironmentVariables(string configFilePath, string destinationFilePath = null)
        {
            string customConfig = GetFullPath(configFilePath);
            string defaultEpiConfig = null;

            if (!string.IsNullOrWhiteSpace(destinationFilePath))
            {
                defaultEpiConfig = GetFullPath(destinationFilePath);
            }

            var doc = XDocument.Load(customConfig);
            foreach (var element in doc.XPathSelectElements(".//level"))
            {
                var valueAttribute = element.Attribute("value");
                var value = Regex.Match(valueAttribute.Value);

                var environmentVariable = Environment.GetEnvironmentVariable(value.Groups["key"].Value);
                if (!string.IsNullOrWhiteSpace(environmentVariable))
                {
                    valueAttribute.SetValue(environmentVariable);
                }
            }

            doc.Save(string.IsNullOrWhiteSpace(defaultEpiConfig) ? customConfig : defaultEpiConfig);
        }

        private string GetFullPath(string relativePath)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(baseDirectory, relativePath);
        }
    }
}