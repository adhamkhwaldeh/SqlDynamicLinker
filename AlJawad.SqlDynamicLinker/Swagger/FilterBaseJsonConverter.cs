using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using AlJawad.SqlDynamicLinker.Models;

//namespace AlJawad.SqlDynamicLinker.Swagger
//{
//    public class FilterBaseJsonConverter : JsonConverter<EntityBaseFilter>
//    {
//        public override EntityBaseFilter? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            using var doc = JsonDocument.ParseValue(ref reader);
//            if (!doc.RootElement.TryGetProperty("$type", out var typeProp))
//                throw new JsonException("$type property missing");

//            return typeProp.GetString() switch
//            {
//                "EntityFilter" => JsonSerializer.Deserialize<EntityFilter>(doc.RootElement.GetRawText(), options),
//                "EntityMultilpleConditionsFilter" => JsonSerializer.Deserialize<EntityMultilpleConditionsFilter>(doc.RootElement.GetRawText(), options),
//                "EntityGeometryFilter" => JsonSerializer.Deserialize<EntityGeometryFilter>(doc.RootElement.GetRawText(), options),
//                _ => throw new JsonException("Unknown $type")
//            };
//        }

//        public override void Write(Utf8JsonWriter writer, EntityBaseFilter value, JsonSerializerOptions options)
//        {
//            if (value is EntityFilter s)
//            {
//                writer.WriteStartObject();
//                writer.WriteString("$type", "EntityFilter");
//                writer.WriteString("DataName", s.DataName);
//                writer.WriteString("Operator", s.Operator);
//                //writer.WriteNullValue("Value", s.Value);
//                writer.WriteEndObject();
//            }
//            else if (value is EntityMultilpleConditionsFilter n)
//            {
//                writer.WriteStartObject();
//                writer.WriteString("$type", "EntityMultilpleConditionsFilter");
//                writer.WriteString("DataName", n.DataName);
//                writer.WriteString("Operator", n.DataName);
//                //writer.WriteNullValue("Value", s.Value);
//                writer.WriteEndObject();
//            }
//            else if (value is EntityGeometryFilter m)
//            {
//                writer.WriteStartObject();
//                writer.WriteString("$type", "EntityGeometryFilter");
//                writer.WriteString("DataName", m.DataName);
//                writer.WriteString("Operator", m.DataName);
//                //writer.WriteNullValue("Value", s.Value);
//                writer.WriteEndObject();
//            }
//        }
//    }
//}