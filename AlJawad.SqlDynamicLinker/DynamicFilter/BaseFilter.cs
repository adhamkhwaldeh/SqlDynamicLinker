using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using AlJawad.SqlDynamicLinker.Models;

namespace AlJawad.SqlDynamicLinker.DynamicFilter
{
    public  class BaseFilter
    {

        public IEnumerable<EntityBaseFilter> DynamicFilters { get; set; } = new List<EntityFilter>();

        public IEnumerable<ColumnBase> IncludeProperties { get; set; } //= new List<ColumnBase>();

        public BaseFilter()
        {

        }

        public BaseFilter(IEnumerable<EntityBaseFilter> dynamicFilters)
        {
            this.DynamicFilters = dynamicFilters;
        }

        public BaseFilter(IEnumerable<ColumnBase> includeProperties)
        {
            this.IncludeProperties = includeProperties;
        }

        public BaseFilter(IEnumerable<EntityBaseFilter> dynamicFilters, IEnumerable<ColumnBase> includeProperties)
        {
            this.DynamicFilters = dynamicFilters;
            this.IncludeProperties = includeProperties;
        }

        //public IEnumerable<Claim> Claims { get; set; }
        //public EntityFilter DynamicFilter { get; set; }

        //public string propertyString
        //{
        //    get
        //    {
        //        try
        //        {
        //            var propertyList = new List<string>();
        //            foreach (var property in IncludeProperties)
        //            {
        //                propertyList.Add(property.columnName);
        //            }
        //            var propertyString = propertyList.Aggregate((x, y) => x + "," + y);
        //            return propertyString;
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

    }
}