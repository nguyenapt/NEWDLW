using System.IO;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services
{
    public interface ISystemConfiguratorImporter
    {
        /// <summary>
        ///     Process all xml files and import into database.
        /// </summary>
        /// <param name="stream">Zipfile that contains all xml files.</param>
        SystemConfiguratorImportResult Import(Stream stream);
    }
}