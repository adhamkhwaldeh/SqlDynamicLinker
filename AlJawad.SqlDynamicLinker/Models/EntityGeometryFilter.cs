using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Models
{
    public class EntityGeometryFilter : EntityBaseFilter
    {

        public string Operator { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Radius { get; set; } //30000;  30 km in geometry units (must match coordinate system)

        public string NamePropertyOfCollection { get; set; }


    }
}