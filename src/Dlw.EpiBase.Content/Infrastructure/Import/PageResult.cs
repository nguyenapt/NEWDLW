using System.Collections.Generic;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    public class PageResult
    {
        public object SourceInfo { get; set; }

        /// <summary>
        /// Path of imported page.
        /// </summary>
        public string Path { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}