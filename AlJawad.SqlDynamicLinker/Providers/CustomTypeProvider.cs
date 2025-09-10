using NetTopologySuite.Geometries;
using ProperMan.Infrastructure.Extensions;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using NetTopologySuite.Geometries;
using AlJawad.SqlDynamicLinker.Extensions;

public class CustomTypeProvider : DefaultDynamicLinqCustomTypeProvider
{
    public override HashSet<Type> GetCustomTypes()
    {
        var types = base.GetCustomTypes();
        types.Add(typeof(DbContextCustomFunctions));
        types.Add(typeof(SpatialFunctions));

        //types.Add(typeof(Geometry));  // <- Needed for Distance, Intersects, etc.

        return types;
    }
}