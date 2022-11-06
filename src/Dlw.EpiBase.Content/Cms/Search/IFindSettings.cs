using System;

namespace Dlw.EpiBase.Content.Cms.Search
{
    public interface IFindSettings
    {
        TimeSpan CacheDuration { get; }

        int MaxItemsPerRequest { get; }
    }
}