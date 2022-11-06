using Castle.Core.Internal;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dlw.EpiBase.Content.Cms.Search
{
    public static class ITypeSearchExtensions
    {
        public static ITypeSearch<T> BuildSorting<T, TProperty>(this ITypeSearch<T> query, Dictionary<Expression<Func<T, TProperty>>, SortOrder> sortings)
            where T : IContentData
        {
            if (sortings.IsNullOrEmpty()) return query;
            var idx = 0;
            foreach (var sorting in sortings)
            {
                var body = sorting.Key.Body as MemberExpression;

                if (body == null && sorting.Key.Body is UnaryExpression)
                {
                    body = ((UnaryExpression)sorting.Key.Body).Operand as MemberExpression;
                }

                if (body == null)
                    continue;

                var propertyInfo = (PropertyInfo)body.Member;

                var propertyType = propertyInfo.PropertyType;

                query = query.Sort(sorting, propertyInfo, propertyType, idx);

                idx++;
            }
            return query;
        }

        public static ITypeSearch<T> Sort<T, TProperty>(this ITypeSearch<T> query,
            KeyValuePair<Expression<Func<T, TProperty>>,
            SortOrder> sorting, PropertyInfo propertyInfo, Type propertyType,
            int sequence)
            where T : IContentData
        {
            if (propertyType == typeof(string))
            {
                var expression = RewriteExpression<T, string>(propertyInfo);

                if (sequence == 0)
                {
                    return sorting.Value == SortOrder.Ascending ? query.OrderBy(expression) : query.OrderByDescending(expression);
                }
                else
                {
                    return sorting.Value == SortOrder.Ascending ? query.ThenBy(expression) : query.ThenByDescending(expression);
                }
            }

            if (propertyType == typeof(DateTime))
            {
                var expression = RewriteExpression<T, DateTime>(propertyInfo);

                if (sequence == 0)
                {
                    return sorting.Value == SortOrder.Ascending ? query.OrderBy(expression) : query.OrderByDescending(expression);
                }
                else
                {
                    return sorting.Value == SortOrder.Ascending ? query.ThenBy(expression) : query.ThenByDescending(expression);
                }
            }

            if (propertyType == typeof(int))
            {
                var expression = RewriteExpression<T, int>(propertyInfo);

                if (sequence == 0)
                {
                    return sorting.Value == SortOrder.Ascending ? query.OrderBy(expression) : query.OrderByDescending(expression);
                }
                else
                {
                    return sorting.Value == SortOrder.Ascending ? query.ThenBy(expression) : query.ThenByDescending(expression);
                }
            }

            return query;
        }

        private static Expression<Func<T, TProperty>> RewriteExpression<T, TProperty> (PropertyInfo propertyInfo)
        {
            ParameterExpression entityParam = Expression.Parameter(typeof(T), "e");

            Expression convertedEx = null;
            Expression columnExpr = null;

            if (propertyInfo.DeclaringType != typeof(T))
            {
                convertedEx = Expression.Convert(entityParam, propertyInfo.DeclaringType);
                columnExpr = Expression.Property(convertedEx, propertyInfo);
            }
            else
            {
                columnExpr = Expression.Property(entityParam, propertyInfo);
            }
            

            if (propertyInfo.PropertyType != typeof(TProperty))
                columnExpr = Expression.Convert(columnExpr, typeof(TProperty));

            return Expression.Lambda<Func<T, TProperty>>(columnExpr, entityParam);
        }
    }
}
