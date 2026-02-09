using AlJawad.SqlDynamicLinker.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Index.HPRtree;

namespace AlJawad.SqlDynamicLinker.Converters
{
    public class EntityBaseFilterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EntityBaseFilter);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load the JSON into a JObject
            var jo = JObject.Load(reader);

            // Read discriminator

            EntityBaseFilter target;
            var x = jo["NamePropertyOfCollectionList"]?.ToString();
            var x2 = jo["namePropertyOfCollectionList"]?.ToString();

            var y = jo["Latitude"]?.ToString();
            var y2 = jo["latitude"]?.ToString();

            var z = jo["MinLatitude"]?.ToString();
            var z2 = jo["minLatitude"]?.ToString();

            if (!String.IsNullOrEmpty(x) || !String.IsNullOrEmpty(x2))
            {
                target = new EntityMultilpleConditionsFilter();
            }
            else if (!String.IsNullOrEmpty(y) || !String.IsNullOrEmpty(y2))
            {
                target = new EntityGeometryFilter();
            }
            else if (!String.IsNullOrEmpty(z) || !String.IsNullOrEmpty(z2))
            {
                target = new EntityBoundingBoxFilter();
            }
            else
            {
                target = new EntityFilter();
            }
            // Populate the target object
            serializer.Populate(jo.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Default serialization is fine
            JObject jo = JObject.FromObject(value, serializer);

            // Add discriminator if missing
            //if (value is StringFilter) jo.AddFirst(new JProperty("filterType", "string"));
            //else if (value is NumberFilter) jo.AddFirst(new JProperty("filterType", "number"));

            jo.WriteTo(writer);
        }
    }

}
