using Flurl.Util;
using Microsoft.EntityFrameworkCore.Internal;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using AlJawad.SqlDynamicLinker.Models;
using AlJawad.SqlDynamicLinker.Enums;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using AlJawad.SqlDynamicLinker.Extensions;
using Azure.Core.GeoJson;

namespace AlJawad.SqlDynamicLinker.Core
{
    public class LinqExpressionBuilder
    {
        private static readonly IDictionary<string, string> _operatorMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
           {
               { EntityFilterOperators.Equal, "==" },
               { EntityFilterOperators.NotEqual, "!=" },
               { EntityFilterOperators.LessThan, "<" },
               { EntityFilterOperators.LessThanOrEqual, "<=" },
               { EntityFilterOperators.GreaterThan, ">" },
               { EntityFilterOperators.GreaterThanOrEqual, ">=" }
           };

        private readonly StringBuilder _expression = new StringBuilder();
        private readonly List<object> _values = new List<object>();

        public IReadOnlyList<object> Parameters => _values;
        public string Expression => _expression.ToString();

        public void Build(EntityBaseFilter entityFilter)
        {
            ResetState();
            if (entityFilter != null)
            {
                Visit(entityFilter);
            }
        }

        public void Build(IEnumerable<EntityBaseFilter> filters)
        {
            ResetState();
            if (filters != null && filters.Any())
            {
                Visit(new EntityFilter()
                {

                    Filters = filters
                });
            }
        }

        private void ResetState()
        {
            _expression.Clear();
            _values.Clear();
        }

        private void Visit(EntityBaseFilter entityFilter)
        {
            if (entityFilter == null) return;

            switch (entityFilter)
            {
                case EntityFilter filter:
                    WriteExpression(filter);
                    break;
                case EntityGeometryFilter filter:
                    WriteExpression(filter);
                    break;
                case EntityMultilpleConditionsFilter multipleConditionsFilter:
                    WriteExpression(multipleConditionsFilter);
                    break;
                case EntityBoundingBoxFilter filter:
                    WriteExpression(filter);
                    break;
            }

            if (entityFilter.Filters != null && entityFilter.Filters.Any())
            {
                //if (!String.IsNullOrEmpty(entityFilter.LogicOrDefault))
                //{
                _expression.Append($" {entityFilter.LogicOrDefault} (");
                //}

                foreach (var filter in entityFilter.Filters)
                {
                    Visit(filter);

                    if (filter != entityFilter.Filters.Last())
                    {
                        _expression.Append($" {filter.LogicOrDefault} ");
                    }
                }

                //if (!String.IsNullOrEmpty(entityFilter.LogicOrDefault))
                //{
                _expression.Append(")");
                //}
            }
        }

        private void WriteExpression(EntityFilter entityFilter)
        {
            if (string.IsNullOrWhiteSpace(entityFilter.DataName))
                return;

            var index = _values.Count;
            var name = entityFilter.DataName;
            var value = NormalizeValue(entityFilter);

            var comparison = GetComparisonOperator(entityFilter.Operator);
            var negation = GetNegation(comparison);

            if (comparison.EndsWith(EntityFilterOperators.StartsWith, StringComparison.OrdinalIgnoreCase))
            {
                _expression.Append($"{negation}{name}.StartsWith(@{index})");
            }
            else if (comparison.EndsWith(EntityFilterOperators.EndsWith, StringComparison.OrdinalIgnoreCase))
            {
                _expression.Append($"{negation}{name}.EndsWith(@{index})");
            }
            else if (comparison.EndsWith(EntityFilterOperators.Contains, StringComparison.OrdinalIgnoreCase))
            {
                HandleContainsExpression(entityFilter, name, index, negation, value);
            }
            else
            {
                _expression.Append($"{name} {comparison} @{index}");
            }

            _values.Add(value);
        }

        private void WriteExpression(EntityGeometryFilter entityFilter)
        {
            //if (string.IsNullOrWhiteSpace(entityFilter.DataName) || entityFilter.NamePropertyOfCollection == null)
            if (string.IsNullOrWhiteSpace(entityFilter.DataName))
                return;

            int srid = 4326; //0 or 4326 if using lat/lon

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: srid);
            var center = geometryFactory.CreatePoint(new Coordinate(entityFilter.Longitude, entityFilter.Latitude));

            //var center = new Point(11.573, 48.137) { SRID = srid };

            // Check distance to MultiPoint/Polygon/LineString geometry
            //var tmpExpression = new StringBuilder($"SpatialFunctions.Distance({entityFilter.DataName},@{_values.Count}) < @{_values.Count + 1}");


            StringBuilder tmpExpression = (entityFilter.IsMultiPoint ?
               new StringBuilder($"{entityFilter.DataName}.Any(x => x.IsWithinDistance(@{_values.Count}, @{_values.Count + 1}))")
             : new StringBuilder($"{entityFilter.DataName}.IsWithinDistance(@{_values.Count}, @{_values.Count + 1})"));

            _expression.Append(tmpExpression);
            _values.Add(center);
            _values.Add(entityFilter.Radius);
        }

        private void WriteExpression(EntityBoundingBoxFilter entityFilter)
        {
            if (string.IsNullOrWhiteSpace(entityFilter.DataName))
                return;

            int srid = 4326;
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: srid);

            var coords = new Coordinate[]
            {
                new Coordinate(entityFilter.MinLongitude, entityFilter.MinLatitude),
                new Coordinate(entityFilter.MaxLongitude, entityFilter.MinLatitude),
                new Coordinate(entityFilter.MaxLongitude, entityFilter.MaxLatitude),
                new Coordinate(entityFilter.MinLongitude, entityFilter.MaxLatitude),
                new Coordinate(entityFilter.MinLongitude, entityFilter.MinLatitude)
            };
            var bbox = geometryFactory.CreatePolygon(coords);

            StringBuilder tmpExpression = (entityFilter.IsMultiPoint ?
               new StringBuilder($"{entityFilter.DataName}.Any(x => x.Intersects(@{_values.Count}))")
             : new StringBuilder($"{entityFilter.DataName}.Intersects(@{_values.Count})"));

            _expression.Append(tmpExpression);
            _values.Add(bbox);
        }
    
        // var tmpExpression = new StringBuilder($" cast({entityFilter.DataName}, Geometry).IsWithinDistance(@{_values.Count}, @{_values.Count + 1})");

      

        private void WriteExpression(EntityMultilpleConditionsFilter entityFilter)
        {
            if (string.IsNullOrWhiteSpace(entityFilter.DataName) || entityFilter.NamePropertyOfCollectionList == null)
                return;
            var paramName = "x" + (_values.Count() + 1);
            var tmpExpression = new StringBuilder($"{entityFilter.DataName}.Any(");//{paramName} =>
            for (int i = 0; i < entityFilter.NamePropertyOfCollectionList.Count; i++)
            {
                var nameProperty = entityFilter.NamePropertyOfCollectionList[i];
                var comparison = entityFilter.OperatorList[i];
                var value = entityFilter.ValueList[i];
                var logic = entityFilter.InnerLogicList[i];

                if (value is JArray jArray)
                {
                    //tmpExpression.Append($"@{_values.Count}.Contains({paramName}.{nameProperty})");
                    tmpExpression.Append($"@{_values.Count}.Contains({nameProperty})");
                    value = NormalizeValue(new EntityFilter() { Value = value });// jArray.ToObject<List<object>>().ToArray();
                }
                else
                {
                    tmpExpression.Append($"@{nameProperty} {comparison} @{_values.Count}");
                    //tmpExpression.Append($"@{paramName}.{nameProperty} {comparison} @{_values.Count}");
                }

                if (i < entityFilter.NamePropertyOfCollectionList.Count - 1)
                {
                    tmpExpression.Append($" {logic} ");
                }
                else
                {
                    tmpExpression.Append(" )");
                }

                _values.Add(value);
            }

            _expression.Append(tmpExpression);
        }

        private object NormalizeValue(EntityFilter entityFilter)
        {
            if (entityFilter.Value is JArray jArray)
            {
                if (!jArray.Any())
                    return Array.Empty<object>(); // or null, depending on your needs

                var first = jArray.First!;

                return first.Type switch
                {
                    JTokenType.Integer =>
                        // Decide between long or int based on value range
                        entityFilter.IsLongArray
                            ? jArray.ToObject<long[]>()
                            : jArray.ToObject<int[]>(),

                    JTokenType.Float => jArray.ToObject<double[]>(),
                    JTokenType.String => jArray.ToObject<string[]>(),
                    JTokenType.Boolean => jArray.ToObject<bool[]>(),
                    _ => jArray.ToObject<object[]>()
                };

            }

            return entityFilter.Value;
        }

        private string GetComparisonOperator(string operatorValue)
        {
            if (string.IsNullOrWhiteSpace(operatorValue))
                return "==";

            return _operatorMap.TryGetValue(operatorValue, out var comparison) ? comparison : operatorValue.Trim();
        }

        private string GetNegation(string comparison)
        {
            return comparison.StartsWith("!") || comparison.StartsWith("not", StringComparison.OrdinalIgnoreCase) ? "!" : string.Empty;
        }


        //Plan
     


        private void HandleContainsExpression(EntityFilter entityFilter, string name, int index, string negation,object value)
        {
            // If DataName is a nested collection path and NamePropertyOfCollection is set, handle dynamically
            if (!string.IsNullOrWhiteSpace(entityFilter.NamePropertyOfCollection) && name.Contains('.'))
            {
                var collections = name.Split('.');
                var sb = new StringBuilder();
                var paramNames = new List<string>();

                // Build parameter names for each level
                for (int i = 0; i <= collections.Length; i++)
                    paramNames.Add("x" + i);

                // Start with the outermost collection
                sb.Append(negation+collections[0]);
                // Build nested Any for each collection except the last
                for (int i = 1; i < collections.Length; i++)
                {
                    sb.Append($".Any({paramNames[i]} => {paramNames[i]}.{collections[i]}");
                }
                // Innermost: check if the property is in the values
                string lastParam = paramNames[collections.Length];
                sb.Append($".Any({lastParam} => @{index}.Contains({lastParam}.{entityFilter.NamePropertyOfCollection}))");

                // Close all opened parentheses
                for (int i = 1; i < collections.Length; i++)
                    sb.Append(")");

                _expression.Append(sb.ToString());
                _values.Add(value);
                return;
            }

            // Fallback to original logic for single collection or property
            if (!string.IsNullOrWhiteSpace(entityFilter.NamePropertyOfCollection))
            {
                _expression.Append($"{name}.Any(@{index}.Contains({entityFilter.NamePropertyOfCollection}))");
            }
            else if (entityFilter.Value is JArray || entityFilter.Value is Array)
            {
                _expression.Append($"{negation}@{index}.Contains({name})");
            }
            else
            {
                _expression.Append($"{negation}{name}.Contains(@{index})");
            }
            //_values.Add(entityFilter.Value);
            _values.Add(value);
        }

    }
}