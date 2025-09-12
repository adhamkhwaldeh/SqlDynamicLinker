using DataTables.AspNetCore.Mvc.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.DynamicFilter;
using Microsoft.IdentityModel.Tokens;
using AlJawad.SqlDynamicLinker.Models;
using AlJawad.SqlDynamicLinker.Enums;

namespace ProperMan.Domain.Profiles
{
   
    public class DataTablesRequestPagingFilterProfile : AutoMapper.Profile
    {
        public DataTablesRequestPagingFilterProfile()
        {

            CreateMap<DataTablesRequest, BasePagingFilter>()
             .ForMember(d => d.Query, m => m.MapFrom(m => m.Search != null ? m.Search.Value : null))
             .ForMember(d => d.PageSize, m => m.MapFrom(m => m.Length))
             .ForMember(d => d.Page, m => m.MapFrom((src, dest) =>
              {
                  return (src.Start / src.Length) + 1;
              }))
             .ForMember(d => d.DynamicSorting, m => m.MapFrom((src, dest) =>
             {
                 List<EntityColumnSort> destinationValue = new List<EntityColumnSort>();
                 foreach (var sort in src.Orders.Select((value, i) => (value, i)))
                 {
                     destinationValue.Add(new EntityColumnSort()
                     {
                         SortIndex = sort.i,
                         DataName = src.Columns.ElementAt(sort.value.Column).Name,
                         IsAscending = sort.value.Dir == EntitySortDirections.Ascending,
                     });
                 }
                 return destinationValue;
             }))
             .ForMember(d => d.DynamicFilters, m => m.MapFrom((src, dest) =>
             {
                 List<EntityFilter> destinationValue = new List<EntityFilter>();
                 foreach (var column in src.Columns)
                 {
                     if(column.Searchable && column.SearchValue!= null && ! String.IsNullOrEmpty(column.SearchValue))
                     {
                         destinationValue.Add(new EntityFilter()
                         {
                             DataName = column.Name,
                             Value = column.SearchValue,
                             Operator = column.SearchRegEx ? EntityFilterOperators.Equal : EntityFilterOperators.Contains,
                             Logic = EntityFilterLogic.And,
                         });
                     }
                 }
                 return destinationValue;
             }));

        }

    }
}