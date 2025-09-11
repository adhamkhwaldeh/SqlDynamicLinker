using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlJawad.SqlDynamicLinker.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder { Asc, Desc }
}
