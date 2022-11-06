using System;
using System.Collections.Generic;

namespace Dlw.EpiBase.Content.Infrastructure.Collections
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public IEnumerable<T> Items { get; }

        public int CurrentPage { get; }

        public int TotalCount { get; }

        public PagedResult(IEnumerable<T> items, int currentPage, int totalCount)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (currentPage < 0) throw new ArgumentOutOfRangeException(nameof(currentPage), "Must be greater or equal than 0.");
            if (totalCount < 0) throw new ArgumentOutOfRangeException(nameof(totalCount), "Must be greater or equal than 0.");

            Items = items;
            CurrentPage = currentPage;
            TotalCount = totalCount;
        }
    }
}