using System.IO;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    /// <summary>
    /// Translates the result of an import into a format that can be stored and read by the end user.
    /// </summary>
    public interface IImportResultFormatter
    {
        Stream Process(ImportResult result);
    }
}