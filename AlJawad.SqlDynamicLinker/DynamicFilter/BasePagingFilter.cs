using Microsoft.AspNetCore.Mvc;
using ProperMan.Infrastructure.ModelBinder;
using System;
using System.Collections.Generic;
using System.Text;
using AlJawad.SqlDynamicLinker.Models;

namespace AlJawad.SqlDynamicLinker.DynamicFilter
{

    [ModelBinder(BinderType = typeof(BasePagingFilterModelBinder))]
    public class BasePagingFilter : BaseQueryableFilter
    {
        public int Page { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public BasePagingFilter()
        {

        }

        public BasePagingFilter(int Page, int PageSize, string query, List<EntityColumnSort> columns,
            IEnumerable<EntityFilter> dynamicFilters, IEnumerable<ColumnBase> includeProperties)
            : base(query, columns, dynamicFilters, includeProperties)
        {
            this.Page = Page;
            this.PageSize = PageSize;
        }

    }

}