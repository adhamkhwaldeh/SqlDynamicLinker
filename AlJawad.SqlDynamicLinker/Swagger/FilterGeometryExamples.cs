using AlJawad.SqlDynamicLinker.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Swagger
{
    public class FilterGeometryExamples : IExamplesProvider<EntityGeometryFilter>
    {
        public EntityGeometryFilter GetExamples()
        {
            return new EntityGeometryFilter
            {
                Logic = "AND",
                Filters = new List<EntityBaseFilter>(),
                //    Filters = new List<EntityBaseFilter>
                //{
                //    new EntityMultilpleConditionsFilter {
                //    DataName="",
                //    InnerLogicList={ "And","OR"},
                //    OperatorList = { },
                //    ValueList = {"","" },
                //  Filters   = []
                //    },
                //    new EntityGeometryFilter {
                //        DataName ="",Latitude=0.0,Longitude=0.0,Radius=0.0,
                //        Filters = []
                //    },
                //    },

                DataName = "exampleData",
                Latitude = 0.0,
                Longitude = 0.0,
            };
        }
    }
}