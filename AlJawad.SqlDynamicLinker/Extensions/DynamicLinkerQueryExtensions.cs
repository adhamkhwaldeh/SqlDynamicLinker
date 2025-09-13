using AlJawad.SqlDynamicLinker.Enums;
using AlJawad.SqlDynamicLinker.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer;
using ProperMan.Infrastructure.Extensions;
using System.Text.Json.Nodes;

namespace AlJawad.SqlDynamicLinker.Extensions
{
    public static class DynamicLinkerQueryExtensions
    {

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, EntityColumnSort sort)
        {
            return sort == null ? query : Sort(query, new[] { sort });
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, IEnumerable<EntityColumnSort> sorts)
        {
            if (sorts == null || !sorts.Any())
                return query;

            // Create ordering expression e.g. Field1 asc, Field2 desc
            var builder = new StringBuilder();
            foreach (var sort in sorts)
            {
                if (builder.Length > 0)
                    builder.Append(",");
                if (sort.IsJsonProperty)
                {
                    //builder.Append(" CASE WHEN JSON_VALUE(["+sort.DataName+"], '$."+sort.JsonIsJsonProperty+ "') IS NULL THEN 1 ELSE 0 END, JSON_VALUE(["+sort.DataName+ "], '$."+sort.JsonIsJsonProperty+ "')").Append(" ");
                    //builder.Append(" iif(JSON_VALUE([" + sort.DataName + "], '$." + sort.JsonIsJsonProperty + "') is null, 1, 0) , JSON_VALUE([" + sort.DataName + "], '$." + sort.JsonIsJsonProperty + "')").Append(" ");
                    //builder.Append(" JSON_VALUE([" + sort.DataName + "], '$." + sort.JsonIsJsonProperty + "')").Append(" ");
                    //builder.Append(" "+ sort.DataName + "[\"" + sort.JsonIsJsonProperty + "\"]").Append(" ");
                    //builder.Append(" " + sort.DataName + "[\"" + sort.JsonIsJsonProperty + "\"]").Append(" ");
                    ////builder.Append(" " + sort.DataName + "[\"" + sort.JsonIsJsonProperty + "\"]").Append(" ");
                    //builder.Append(" " +  "JSON_VALUE(" + sort.DataName + ", \"$."+ sort.JsonIsJsonProperty + "\")").Append(" ");
                    // return query.OrderBy(@"it." + sort.DataName + "." + sort.JsonIsJsonProperty);
                    //return query.OrderBy(@"it."+ sort.DataName +"[\""+ sort.JsonIsJsonProperty + "\"]");
                    // ss.Set<JsonEntityBasic>().Where(c => EF.Functions.JsonValue(c.OwnedReferenceRoot, "$.Name") == "foo")
                    //DbContextCustomFunctions.JsonValue("x", "");
                    var jsonProp = $"DbContextCustomFunctions.JsonValue({sort.DataName}, \"$.{sort.JsonIsJsonProperty}\")";
                    builder.Append($" iif( {jsonProp} = null, 1, 0) , {jsonProp}").Append(" ");
                    //builder.Append($"iif(DbContextCustomFunctions.JsonValue({sort.DataName}, \"$.{sort.JsonIsJsonProperty}\") = null, 1, 0) ");
                    //builder.Append($"DbContextCustomFunctions.JsonValue({sort.DataName}, \"$.{sort.JsonIsJsonProperty}\")").Append(" ");

                    //builder.Append($"JSON_VALUE({sort.DataName}, \"$.{sort.JsonIsJsonProperty}\")").Append(" ");
                    //builder.Append($"EF.Functions.JsonValue({sort.DataName}, '$.{sort.JsonIsJsonProperty}')").Append(" ");

                    //builder.Append($"EF.Functions.JsonValue({sort.DataName}, '$.{sort.JsonIsJsonProperty})").Append(" ");
                    ///builder.Append($"EF.Functions.JsonValue(" +sort.DataName+", \"$." + sort.JsonIsJsonProperty+ "\")").Append(" ");
                    //builder.Append(EF.Functions.JsonValue(" + sort.DataName + ", '$."+sort.JsonIsJsonProperty+"')").Append(" ");

                }
                else
                {
                    builder.Append(sort.DataName).Append(" ");
                }
                //var isDescending =  !string.IsNullOrWhiteSpace(sort.Direction)
                //    && sort.Direction.StartsWith(EntitySortDirections.Descending, StringComparison.OrdinalIgnoreCase);
                builder.Append(sort.IsAscending ? EntitySortDirections.Ascending : EntitySortDirections.Descending);
            }
            //DynamicFunctions.
            //query.Where(x => EF.Functions.JsonValue("x", ""));
            var config = new ParsingConfig
            {
                CustomTypeProvider = new CustomTypeProvider()

                //ResolveTypesBySimpleName = true,
            };
            //return query.OrderBy(ParsingConfig.DefaultEFCore21, builder.ToString()).to;

            //query.OrderBy(config, builder.ToString()).ToList();
            return query.OrderBy(config,builder.ToString());
        }


        public static IQueryable<T> Includes<T>(this IQueryable<T> query, ColumnBase columnBase)
            where T : class
        {
            return columnBase == null ? query : Includes(query, new[] { columnBase });
        }

        public static IQueryable<T> Includes<T>(this IQueryable<T> query, IEnumerable<ColumnBase> includes)
                 where T : class
        {
          if (includes == null)
                return query;
          return  includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty.DataName));
        }



        public static IQueryable<T> IncludeIf<T>(this IQueryable<T> source, bool condition, string path)
           where T : class
        {
            return condition
                ? source.Include(path)
                : source;
        }

        public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> path)
          where T : class
        {
            return condition
                ? source.Include(path)
                : source;
        }

        public static IQueryable<T> IncludeIf<T>(
           this IQueryable<T> source,
           bool condition,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
           where T : class
        {
            return condition
                ? include(source)
                : source;
        }

        public static void AddRange<T>(this ICollection<T> destination,
                               IEnumerable<T> source)
        {
            List<T> list = destination as List<T>;

            if (list != null)
            {
                list.AddRange(source);
            }
            else
            {
                foreach (T item in source)
                {
                    destination.Add(item);
                }
            }
        }

    }
}
