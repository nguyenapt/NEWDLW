using System.Collections.Generic;

namespace Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator
{
    public interface IContentGeneratorService
    {
        IEnumerable<ContentGeneratorResult> Generate(ContentContext contentContext);
    }
}