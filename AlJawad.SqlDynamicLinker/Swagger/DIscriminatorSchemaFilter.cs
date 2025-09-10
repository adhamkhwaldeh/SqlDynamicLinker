using AlJawad.SqlDynamicLinker.Models;
using Microsoft.OpenApi.Models;
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
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
           if(context.Type == typeof(EntityBaseFilter))
            {
               // schema.Description =
               //"This is a polymorphic type. Use 'type' = 'dog' or 'cat' to indicate the subtype.";

                schema.OneOf = new List<OpenApiSchema>
            {
                context.SchemaGenerator.GenerateSchema(typeof(EntityFilter), context.SchemaRepository),
                context.SchemaGenerator.GenerateSchema(typeof(EntityMultilpleConditionsFilter), context.SchemaRepository),
                context.SchemaGenerator.GenerateSchema(typeof(EntityGeometryFilter), context.SchemaRepository)
            };

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
                schema.Items = context.SchemaGenerator.GenerateSchema(typeof(EntityBaseFilter), context.SchemaRepository);
            }
        }
    }
}
