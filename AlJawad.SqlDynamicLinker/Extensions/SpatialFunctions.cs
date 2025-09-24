using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using NetTopologySuite.Geometries;
using AlJawad.SqlDynamicLinker.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Extensions
{
    public static class SpatialFunctions
    {
        public static double Distance(Geometry geom1, Geometry geom2)
        => throw new NotImplementedException(); // EF Core will translate this    

        public static ModelBuilder AddCustomSpatialFunctions(this ModelBuilder modelBuilder)
        {
            var distanceMethod = typeof(SpatialFunctions)
                .GetMethod(nameof(SpatialFunctions.Distance), new[] { typeof(Geometry), typeof(Geometry) });

            var doubleTypeMapping = new DoubleTypeMapping("float", System.Data.DbType.Double);

            modelBuilder.HasDbFunction(distanceMethod)
                .HasTranslation(args =>
                {
                    // args[0] = instance (GeoPoint), args[1] = argument (reference point)    
                    return new SqlFunctionExpression(
                        instance: args[0],               // instance = GeoPoint    
                        name: "STDistance",              // SQL spatial method    
                        schema: null,
                        arguments: new[] { args[1] },    // argument = point    
                        nullable: true,
                        instancePropagatesNullability: true,
                        argumentsPropagateNullability: new[] { true }, // Added required parameter  
                        //isNiladic: false,                // Added required parameter  
                        builtIn:true,
                        type: distanceMethod.ReturnType,
                        typeMapping: doubleTypeMapping
                    );
                });
            return modelBuilder;
        }
    }
}
