using System.Collections.Generic;
using System.Linq;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    public class ImportResult
    {
        public bool Success => ImportedPages.All(p => p.Success);

        // TODO rename to ImportedEntities? / Objects?
        public IEnumerable<PageResult> ImportedPages { get; set; }
    }
}