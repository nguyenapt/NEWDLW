using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    public class PropertyNameSuffixLocalizedDataTransformer : ILocalizedDataTransformer
    {
        private Regex LanguageCodeRegex = new Regex(@"^(?'prop'.*)\[(?'code'[a-zA-Z]{2})\]$");
        private Regex CleanupRegex = new Regex(@"[^0-9a-zA-Z\[\]]+");

        public IEnumerable<DynamicData> Transform(IEnumerable<DynamicData> dataToImport, out IReadOnlyList<CultureInfo> foundLanguages)
        {
            var parsedLanguages = new List<CultureInfo>();
            var transformedData = new List<DynamicData>();

            foreach (var data in dataToImport)
            {
                var localizedProperties = new Dictionary<string, List<string>>();

                foreach (var fieldName in data.FieldNames())
                {
                    string propertyName;
                    string code;
                    if (ContainsLanguageCode(fieldName, out propertyName, out code))
                    {
                        EnsureParsedLanguages(code, parsedLanguages);
                        EnsureProperty(propertyName, code, localizedProperties);
                    }
                }

                if (localizedProperties.Any())
                {
                    transformedData.Add(TransformData(data, localizedProperties));

                    continue;
                }

                transformedData.Add(data);
            }

            foundLanguages = parsedLanguages;

            return transformedData;
        }

        private DynamicData TransformData(DynamicData data, Dictionary<string, List<string>> localizedProperties)
        {
            var transformedData = data;

            foreach (var property in localizedProperties)
            {
                var localizedPropertyValue = new Dictionary<CultureInfo, object>();

                foreach (var language in property.Value)
                {
                    var localizedPropertyName = FormatPropertyName(property.Key, language);

                    localizedPropertyValue.Add(CultureInfo.GetCultureInfo(language), transformedData[localizedPropertyName]);

                    transformedData.Remove(localizedPropertyName);
                }

                transformedData.Add(property.Key, localizedPropertyValue);
            }

            return transformedData;
        }

        private string FormatPropertyName(string propertyKey, string language)
        {
            return $"{propertyKey}[{language}]";
        }

        private bool ContainsLanguageCode(string localizedPropertyName, out string propertyName, out string code)
        {
            if (!LanguageCodeRegex.IsMatch(localizedPropertyName))
            {
                propertyName = null;
                code = null;

                return false;
            }

            var result = LanguageCodeRegex.Match(localizedPropertyName);
            code = result.Groups["code"].Value;

            var property = result.Groups["prop"].Value;
            propertyName = CleanupRegex.Replace(property, "");

            return true;
        }

        private void EnsureProperty(string propertyName, string languageCode, Dictionary<string, List<string>> dictionary)
        {
            if (dictionary.ContainsKey(propertyName))
            {
                dictionary[propertyName].Add(languageCode);
                return;
            }

            dictionary.Add(propertyName, new List<string>() { languageCode });
        }

        private void EnsureParsedLanguages(string code, List<CultureInfo> parsedLanguages)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(code);

            if (parsedLanguages.Contains(cultureInfo))
            {
                return;
            }

            parsedLanguages.Add(cultureInfo);
        }
    }
}