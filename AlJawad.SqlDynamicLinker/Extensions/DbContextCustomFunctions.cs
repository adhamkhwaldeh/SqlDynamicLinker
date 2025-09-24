using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Storage;

namespace AlJawad.SqlDynamicLinker.Extensions
{
    public static class DbContextCustomFunctions
    {
        /*
                 * Define extension functions to handle basic data type conversions
                 */
        public static DateTime? ToSqlDateTime(this string s, int format) => throw new NotSupportedException();
        public static int? ToSqlInt(this string s) => throw new NotSupportedException();
        public static decimal? ToSqlDecimal(this string s) => throw new NotSupportedException();
        /*
          * Define functions to handle working with JSON.  These map to the functions directly in TSQL
          */
        //[DbFunction("JSON_VALUE", IsBuiltIn = true)]
        //public static string JsonValue(string column, [NotParameterized] string path) => throw new NotSupportedException();

        [DbFunction("JSON_VALUE", IsBuiltIn = true)]
        public static string JsonValue(object column, [NotParameterized] string path) => throw new NotSupportedException();


        //[DbFunction("JSON_VALUE", IsBuiltIn = true)]
        //public static string JsonValue(string column, [NotParameterized] string path) => throw new NotSupportedException();

        [DbFunction("JSON_QUERY", IsBuiltIn = true)]
        public static string JsonQuery(string source, [NotParameterized] string path) => throw new NotSupportedException();
        
        [DbFunction("OPENJSON", IsBuiltIn = true)]
        public static string OpenJson(string source, [NotParameterized] string path) => throw new NotSupportedException();
        /// <summary>
        /// Registers tsql functions to EF
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder AddCustomFunctions(this ModelBuilder modelBuilder)
        {
            // the first 3 functions map a string to the TRY_CONVERT function in TSQL
            modelBuilder.HasDbFunction(() => ToSqlDateTime(default, default))
                .HasTranslation(args =>
                    new SqlFunctionExpression(
                        functionName: "TRY_CONVERT",
                        arguments: args.Prepend(new SqlFragmentExpression("datetime2")),
                        nullable: true,
                        argumentsPropagateNullability: new[] { false, true, false },
                        type: typeof(DateTime),
                        typeMapping: null
                    )
                );
            modelBuilder.HasDbFunction(() => ToSqlInt(default))
                .HasTranslation(args =>
                    new SqlFunctionExpression(
                        functionName: "TRY_CONVERT",
                        arguments: args.Prepend(new SqlFragmentExpression("int")),
                        nullable: true,
                        argumentsPropagateNullability: new[] { false, true },
                        type: typeof(int),
                        typeMapping: null
                    )
                );
            modelBuilder.HasDbFunction(() => ToSqlDecimal(default))
                 .HasTranslation(args =>
                     new SqlFunctionExpression(
                         functionName: "TRY_CONVERT",
                         arguments: args.Prepend(new SqlFragmentExpression("decimal")),
                         nullable: true,
                         argumentsPropagateNullability: new[] { false, true },
                         type: typeof(decimal),
                         typeMapping: null
                     )
                 );
            // add support for Json functions
            // modelBuilder.HasDbFunction(() => EF.Functions.JsonValue(default(string), default(string)));
            modelBuilder.HasDbFunction(() => DbContextCustomFunctions.JsonValue(default(string), default(string)));

            //modelBuilder.HasDbFunction(typeof(DbContextCustomFunctions).GetMethod(nameof(DbContextCustomFunctions.JsonValue)))
            //  .HasTranslation(e => SqlFunctionExpression.Create(
            //      "JSON_VALUE", e, typeof(string), null));

            var jsonValueMethod = typeof(DbContextCustomFunctions).GetMethod(nameof(DbContextCustomFunctions.JsonValue));
            var stringTypeMapping = new StringTypeMapping("NVARCHAR(MAX)", System.Data.DbType.String);

            modelBuilder.HasDbFunction(jsonValueMethod)
                           .HasTranslation(args =>
                           {
                           return new SqlFunctionExpression("JSON_VALUE", args, nullable: true, argumentsPropagateNullability: new[] { false, false }, jsonValueMethod.ReturnType, stringTypeMapping);
                               //return SqlFunctionExpression.Create("JSON_VALUE", args, jsonValueMethod.ReturnType, null);
                           })
                .HasParameter("column").Metadata.TypeMapping = stringTypeMapping;

            //modelBuilder.HasDbFunction(jsonValueMethod)
            //    .HasTranslation(args => new SqlFunctionExpression("JSON_VALUE", args, nullable: true, argumentsPropagateNullability: new[] { false, false }, typeof(string), null))
            //    .HasParameter("column").Metadata.TypeMapping = stringTypeMapping;

            //modelBuilder.HasDbFunction(jsonValueMethod)
            //    .HasName("JSON_VALUE") // function name in server
            //    .HasSchema(""); // empty string since in built functions has no schema
                                // 
            modelBuilder.HasDbFunction(() => DbContextCustomFunctions.JsonQuery(default(string), default(string)));
            modelBuilder.HasDbFunction(() => DbContextCustomFunctions.OpenJson(default(string), default(string)));
            return modelBuilder;
        }
    }

}
