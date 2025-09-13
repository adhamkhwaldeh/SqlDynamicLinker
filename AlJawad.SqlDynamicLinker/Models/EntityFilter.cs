using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using AlJawad.SqlDynamicLinker.Models;

namespace AlJawad.SqlDynamicLinker.Models
{
    public class EntityFilter : EntityBaseFilter
    {
        public string Operator { get; set; }

        public object Value { get; set; }

        //Change the list of int to list of long
        public bool IsLongArray { get; set; }

        public string NamePropertyOfCollection { get; set; }



        //public IEnumerable<EntityFilter> Filters { get; set; } = new HashSet<EntityFilter>();
        //public EntityFilter() => Type = "EntityFilter";

        //public override int GetHashCode()
        //{
        //    const int m = -1521134295;
        //    var hashCode = -346447222;

        //    hashCode = hashCode * m + EqualityComparer<string>.Default.GetHashCode(DataName);
        //    hashCode = hashCode * m + EqualityComparer<string>.Default.GetHashCode(Operator);
        //    hashCode = hashCode * m + EqualityComparer<object>.Default.GetHashCode(Value);
        //    hashCode = hashCode * m + EqualityComparer<string>.Default.GetHashCode(Logic);

        //    if (Filters == null)
        //        return hashCode;

        //    foreach (var filter in Filters)
        //        hashCode = hashCode * m + filter.GetHashCode();

        //    return hashCode;
        //}
    }

    //public class ReflectionDynamicObject : DynamicObject
    //{
    //    public JsonElement RealObject { get; set; }

    //    public override bool TryGetMember(GetMemberBinder binder, out object result)
    //    {
    //        // Get the property value
    //        var srcData = RealObject.GetProperty(binder.Name);

    //        result = null;

    //        switch (srcData.ValueKind)
    //        {
    //            case JsonValueKind.Null:
    //                result = null;
    //                break;
    //            case JsonValueKind.Number:
    //                result = srcData.GetDouble();
    //                break;
    //            case JsonValueKind.False:
    //                result = false;
    //                break;
    //            case JsonValueKind.True:
    //                result = true;
    //                break;
    //            case JsonValueKind.Undefined:
    //                result = null;
    //                break;
    //            case JsonValueKind.String:
    //                result = srcData.GetString();
    //                break;
    //            case JsonValueKind.Object:
    //                result = new ReflectionDynamicObject
    //                {
    //                    RealObject = srcData
    //                };
    //                break;
    //            case JsonValueKind.Array:
    //                result = srcData.EnumerateArray()
    //                    .Select(o => new ReflectionDynamicObject { RealObject = o })
    //                    .ToArray();
    //                break;
    //        }

    //        // Always return true; other exceptions may have already been thrown if needed
    //        return true;
    //    }
    //}

}