using AlJawad.SqlDynamicLinker.Enums;
using AlJawad.SqlDynamicLinker.Swagger;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Models
{

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(EntityFilter), "EntityFilter")]
    [JsonDerivedType(typeof(EntityMultilpleConditionsFilter), "EntityMultilpleConditionsFilter")]
    [JsonDerivedType(typeof(EntityGeometryFilter), "EntityGeometryFilter")]
    [JsonDerivedType(typeof(EntityBoundingBoxFilter), "EntityBoundingBoxFilter")]
    //[JsonConverter(typeof(FilterBaseJsonConverter))]

    //[JsonPolymorphic(IgnoreUnrecognizedTypeDiscriminators = true,
    //    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    //[JsonDerivedType(typeof(EntityFilter))]
    //[JsonDerivedType(typeof(EntityMultilpleConditionsFilter))]
    //[JsonDerivedType(typeof(EntityGeometryFilter))]

    public abstract class EntityBaseFilter : ColumnBase
    {

        public string Logic { get; set; }


        //public virtual IEnumerable<EntityBaseFilter> Filters { get; set; } //= new List<EntityBaseFilter>();
        //[SwaggerSchema(  = new List() { EntityFilter(){ })]
        //[SwaggerExample("filter2")]
        //[OpenApiExample(OpenApiExample = new[] { "filter1", "filter2" })]
        public IEnumerable<EntityBaseFilter> Filters { get; set; } = new HashSet<EntityBaseFilter>();
        //public string LogicOrDefault => string.IsNullOrWhiteSpace(Logic) ? EntityFilterLogic.And : Logic;
        public string LogicOrDefault => string.IsNullOrWhiteSpace(Logic) ? "" : Logic;

    }
}