using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Globalization;
using AlJawad.SqlDynamicLinker.Models;
using AlJawad.SqlDynamicLinker.Core;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AlJawad.SqlDynamicLinker.Extensions
{
    public static class DynamicQueryExtensions
    {

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, BaseQueryableFilter filter)
        where T : class
        {
            if (filter == null)
                return query;

            var builder = new LinqExpressionBuilder();
            builder.Build(filter.DynamicFilters);
            query.Sort(filter.DynamicSorting);
            query.Includes(filter.IncludeProperties);
            return builder.Filter(query);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, IEnumerable<EntityBaseFilter> filter)
        {
            if (filter == null)
                return query;

            var builder = new LinqExpressionBuilder();
            builder.Build(filter);
            return builder.Filter(query);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, EntityBaseFilter filter)
        {
            if (filter == null)
                return query;

            var builder = new LinqExpressionBuilder();
            builder.Build(filter);
            return builder.Filter(query);
        }

        public static IQueryable<T> Filter<T>(this LinqExpressionBuilder builder, IQueryable<T> query){
            var predicate = builder.Expression;
            var parameters = builder.Parameters.ToArray();
            if (string.IsNullOrWhiteSpace(predicate))
                return query;

            var config = new ParsingConfig
            {
                ResolveTypesBySimpleName = true,
                AllowNewToEvaluateAnyType = true,

                CustomTypeProvider = new CustomTypeProvider(),
                
                //ResolveTypesBySimpleName = true,
            };

            //xx.NumberParseCulture = CultureInfo.
            //return query.Where("@0.Contains(Id)", intList);
            return query.Where(config, predicate, parameters);
            //.Where(predicate, parameters);
        }

    }
}