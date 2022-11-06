using System.Collections.Generic;

namespace Dlw.EpiBase.Content.Infrastructure.Collections
{
    public interface IPagedResult<T>
    {
        IEnumerable<T> Items { get; }

        int CurrentPage { get; }

        int TotalCount { get; }
    }
}