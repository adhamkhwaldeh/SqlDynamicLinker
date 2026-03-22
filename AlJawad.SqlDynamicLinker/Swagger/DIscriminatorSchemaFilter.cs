using AlJawad.SqlDynamicLinker.Models;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Swagger
{
    public class DiscriminatorSchemaFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
           if(context.Type == typeof(EntityBaseFilter))
            {
               // schema.Description =
               //"This is a polymorphic type. Use 'type' = 'dog' or 'cat' to indicate the subtype.";

                schema.OneOf.Clear();
                schema.OneOf.Add(context.SchemaGenerator.GenerateSchema(typeof(EntityFilter), context.SchemaRepository));
                schema.OneOf.Add(context.SchemaGenerator.GenerateSchema(typeof(EntityMultilpleConditionsFilter), context.SchemaRepository));
                schema.OneOf.Add(context.SchemaGenerator.GenerateSchema(typeof(EntityGeometryFilter), context.SchemaRepository));

                //schema.Discriminator = new OpenApiDiscriminator
                //{
                //    PropertyName = "$type",
                //    Mapping = new Dictionary<string, string>
                //{
                //                { "EntityFilter","#/components/schemas/EntityFilter" },
                //        { "EntityMultilpleConditionsFilter","#/components/schemas/EntityMultilpleConditionsFilter"},
                //    { "stringFilter", "#/components/schemas/EntityGeometryFilter" },
                //}
                //};

                //schema.Discriminator = new OpenApiDiscriminator()
                //{
                //    PropertyName = "type",
                //    Mapping = new Dictionary<string, string>()
                //    {
                //        { "EntityFilter","#/components/schemas/EntityFilter" },
                //        { "EntityMultilpleConditionsFilter","#/components/schemas/EntityMultilpleConditionsFilter"}
                //    }
                //};
                //schema.Required.Add("type");
            }

            // Fix collections of FilterBase
            if (context.Type == typeof(List<EntityBaseFilter>) || context.Type == typeof(IEnumerable<EntityBaseFilter>))
            {
                // In Microsoft.OpenApi v2, IOpenApiSchema properties like Type and Items are read-only on the interface.
                // We cast to the concrete OpenApiSchema to set them. This is the standard implementation used by Swashbuckle.
                if (schema is OpenApiSchema openApiSchema)
                {
                    openApiSchema.Type = JsonSchemaType.Array;
                    openApiSchema.Items = context.SchemaGenerator.GenerateSchema(typeof(EntityBaseFilter), context.SchemaRepository) as OpenApiSchema;
                }
            }
        }
    }
}
