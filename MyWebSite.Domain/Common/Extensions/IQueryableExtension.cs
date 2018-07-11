using MyWebSite.Domain.Common.GridPager;
using MyWebSite.Domain.Common.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyWebSite.Domain.Common.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> table, QueryModel model, string prefix = "") where TEntity : class
        {
            return Where(table, model.Items, prefix);
        }

        private static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> table, IEnumerable<ConditionItem> items, string prefix = "")
        {
            IEnumerable<ConditionItem> filterItems = string.IsNullOrWhiteSpace(prefix)
                ? items.Where(c => string.IsNullOrEmpty(c.Prefix))
                : items.Where(c => c.Prefix == prefix);
            if (filterItems.Count() == 0)
                return table;
            return QueryFactory.Provider.Search(table, filterItems);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool asc)
        {
            var type = typeof(T);
            string methodName = asc ? "OrderBy" : "OrderByDescending";
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> source, string propertyName, bool asc)
        {
            var type = typeof(T);
            string methodName = asc ? "ThenBy" : "ThenByDescending";
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
