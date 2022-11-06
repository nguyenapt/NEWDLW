using System.Collections.Generic;
using System.Globalization;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    public interface ILocalizedDataTransformer
    {
        /// <summary>
        /// Normalizes the localized properties to one property with IDictionary{CultureInfo, object} as value. Which contains the value for each language.
        /// </summary>
        IEnumerable<DynamicData> Transform(IEnumerable<DynamicData> dataToImport, out IReadOnlyList<CultureInfo> foundLanguages);
    }
}