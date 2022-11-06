using System.IO;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    /// <summary>
    /// Repository to manage import related files.
    /// </summary>
    public interface IImportDataRepository
    {
        void Store(string relativePath, Stream data);
    }
}