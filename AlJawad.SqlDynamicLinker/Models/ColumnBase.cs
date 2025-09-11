using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Models
{
    public class ColumnBase
    {
        public String DataName { set; get; } = String.Empty;

        public ColumnBase() { }

        public ColumnBase(string DataName) { this.DataName = DataName; }
    }

}