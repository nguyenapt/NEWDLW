using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EPiServer.Core;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api;
using EPiServer.Find.Cms;

namespace Dlw.EpiBase.Content.Cms.Search
{
    public interface IPageService
    {
        /// <summary>
        /// Returns content reference for specified PageData.
        /// Throws exception when more than one page is found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see cref="ContentReference"/></returns>
        /// <remarks>Does not return deleted pages.</remarks>
        ContentReference GetPageReference<T>() where T : PageData;

        /// <summary>
        /// Returns page for specified PageData.
        /// Throws exception when more than one page is found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks>Does not return deleted pages.</remarks>
        T GetPage<T>() where T : PageData;

        /// <summary>
        /// Returns pages for specific PageData.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagesToFetch"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<T> GetPages<T>(int pagesToFetch, Expression<Func<T, Filter>> filter = null) where T : PageData;

        IContentResult<T> GetContents<T>(int pagesToFetch, Expression<Func<T, Filter>> filter = null, int? skip = null) where T : IContentData;

        IContentResult<T> GetContentsWithSorting<T, TProperty>(int pagesToFetch, Expression<Func<T, Filter>> filter = null,
         Dictionary<Expression<Func<T, TProperty>>, SortOrder> sortings = null, int? skip = null)
         where T : IContentData
         where TProperty : IComparable;
        /// <summary>
        /// Will return the sibling pages of the current Page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// /// <param name="pagesToFetch">Maximum amount of sibling pages that will be returned.</param>
        /// <param name="currentPageReference">The page for which the siblings will be returned.</param>
        /// <returns></returns>
        IEnumerable<T> GetSiblingPages<T>(int pagesToFetch, PageReference currentPageReference) where T : PageData;

        IEnumerable<T> GetPagesWithSorting<T, TProperty>(int pagesToFetch, Expression<Func<T, Filter>> filter,
            Dictionary<Expression<Func<T, TProperty>>, SortOrder> sortings) where T : PageData where TProperty : IComparable;
    }
}