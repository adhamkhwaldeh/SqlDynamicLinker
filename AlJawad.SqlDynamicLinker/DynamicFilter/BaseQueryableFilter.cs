using Microsoft.AspNetCore.Mvc;
using AlJawad.SqlDynamicLinker.ModelBinder;
using System;
using System.Collections.Generic;
using System.Text;
using AlJawad.SqlDynamicLinker.Models;

namespace AlJawad.SqlDynamicLinker.DynamicFilter
{

    [ModelBinder(BinderType = typeof(BaseQueryableFilterModelBinder))]
    public class BaseQueryableFilter: BaseFilter
    {
        public String Query { get; set; } = String.Empty;

        public IList<EntityColumnSort> DynamicSorting { set; get; } = new List<EntityColumnSort>();

        public BaseQueryableFilter()
        {

        }

        public BaseQueryableFilter(string query)
        {
            Query = query;
        }
        public BaseQueryableFilter(string query, List<EntityColumnSort> columns)
        {
            Query = query;
            DynamicSorting = columns;
        }

        public BaseQueryableFilter(string query, List<EntityColumnSort> columns,
            IEnumerable<EntityFilter> dynamicFilters, IEnumerable<ColumnBase> includeProperties)
            : base(dynamicFilters, includeProperties)
        {
            Query = query;
            DynamicSorting = columns;
        }


        //public IEnumerable<EntitySort> Sort
        //{
        //    get
        //    {
        //        List<EntitySort> newColumns = new List<EntitySort>();
        //       foreach (var item in columns){
        //            newColumns.Add(new EntitySort()
        //            {
        //                Name= item.DataName,
        //                Direction = item.OrderDirection
        //            });
        //        }
        //        return newColumns;
        //    }
        //} 

        //#region Below ones for Web
        //public IEnumerable<ColumnBase> allColumns { set; get; } = new List<ColumnBase>();

        //public IEnumerable<ColumnSort> sortColumns { set; get; } = new List<ColumnSort>();

    }
}
