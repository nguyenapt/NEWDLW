using System;
using System.Collections.Generic;
using System.Globalization;
using EPiServer.Core;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    /// <summary>
    /// Supports importing pages for a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPageImporter<T> where T : PageData
    {
        // TODO refactor signature
        event EventHandler PageProcessed;

        ImportResult Import(IEnumerable<DynamicData> pages, IEnumerable<CultureInfo> languages);
    }
}