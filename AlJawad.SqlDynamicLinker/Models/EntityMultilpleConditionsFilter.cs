using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Models
{
    public class EntityMultilpleConditionsFilter : EntityBaseFilter
    {
        public List<string> OperatorList { get; set; }
        public List<object> ValueList { get; set; }
        public List<string> InnerLogicList { get; set; }
        public List<string> NamePropertyOfCollectionList { get; set; }


        //  public EntityMultilpleConditionsFilter() => Type = "EntityMultilpleConditionsFilter";
    }
}