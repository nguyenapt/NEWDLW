using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    public interface IPaginableResult<out TContent> where TContent : IContentData
    {

    }

    public interface IPagedList<out T> : IEnumerable<T> where T : IContentData
    {
        int TotalResult { get; }
        int PageSize { get; }
        int PageNumber { get; }
        int TotalPage { get; }
    }

    public class PagedList<T> : List<T>, IPagedList<T> where T : IContentData
    {
        public PagedList(IEnumerable<T> enumerable, int totalMatchingItems, int pageSize, int pageNumber) : base(enumerable)
        {
            TotalResult = totalMatchingItems;
            PageSize = pageSize;
            PageNumber = pageNumber <= 0 ? 1 : pageNumber;
        }

        public int TotalResult { get; protected set; }

        public int PageSize { get; protected set; }

        public int PageNumber { get; protected set; }

        public int TotalPage
        {
            get
            {
                if (PageSize <= 0)
                    return 0;

                return (int)Math.Ceiling((double)this.TotalResult / this.PageSize);
            }
        }

        public static IPagedList<T> Empty() => new PagedList<T>(Enumerable.Empty<T>(), 0, 1, 1);
    }
}