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

namespace AlJawad.SqlDynamicLinker.DynamicFilter
{
    public static class DynamicQueryExtensions
    {

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, BaseQueryableFilter filter)
        {
            if (filter == null)
                return query;

            var builder = new LinqExpressionBuilder();
            builder.Build(filter.DynamicFilters);
            var predicate = builder.Expression;
            var parameters = builder.Parameters.ToArray();
            if (string.IsNullOrWhiteSpace(predicate))
                return query;

            var config = new ParsingConfig
            {
                ResolveTypesBySimpleName = true,
                AllowNewToEvaluateAnyType = true,
                
                CustomTypeProvider = new CustomTypeProvider()

                //ResolveTypesBySimpleName = true,
            };
            //xx.NumberParseCulture = CultureInfo.
            //return query.Where("@0.Contains(Id)", intList);
            return query.Where(config, predicate, parameters);
                        //.Where(predicate, parameters);
        }

    }
}