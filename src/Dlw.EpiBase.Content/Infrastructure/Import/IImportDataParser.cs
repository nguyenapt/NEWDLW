using System.Collections.Generic;
using System.IO;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    /// <summary>
    /// Parses the data into a format that can be processed by the importer.
    /// </summary>
    public interface IImportDataParser
    {
        bool CanParse(Stream data);

        IEnumerable<DynamicData> Parse(Stream data);
    }
}