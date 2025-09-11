using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.Models;
using AlJawad.SqlDynamicLinker.DynamicFilter;

namespace ProperMan.Infrastructure.ModelBinder
{

    public class BasePagingFilterModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            //get value from bindingContext
            var valueproviders = bindingContext.ValueProvider;
            JsonSerializer serializer = new JsonSerializer();
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = new BasePagingFilter();

            if (valueproviders.GetValue("Query").Length > 0)
            {
                result.Query = valueproviders.GetValue("Query").FirstValue;
            }
            if (valueproviders.GetValue("Page").Length > 0)
            {
                result.Page = Convert.ToInt32(valueproviders.GetValue("Page").FirstValue);
            }
            if (valueproviders.GetValue("PageSize").Length > 0)
            {
                result.PageSize = Convert.ToInt32(valueproviders.GetValue("PageSize").FirstValue);
            }

            if (valueproviders.GetValue("DynamicSorting").Length > 0)
            {   
                var sortlist = new List<EntityColumnSort>();
                foreach (var item in valueproviders.GetValue("DynamicSorting").ToList())
                {
                    var newsort = JsonConvert.DeserializeObject<EntityColumnSort>(item.Replace("\n", ""));
                   //var newsort = JsonSerializer.Deserialize<EntityColumnSort>(JObject.Parse(item.Replace("\n", "")));
                    sortlist.Add(newsort);
                }
                result.DynamicSorting = sortlist;
            }

            if (valueproviders.GetValue("DynamicFilters").Length > 0)
            {
                var dynamicList = new List<EntityBaseFilter>();
                foreach (var item in valueproviders.GetValue("DynamicFilters").ToList())
                {
                    var x = JObject.Parse(item)["NamePropertyOfCollectionList"]?.ToString();
                    var x2 = JObject.Parse(item)["namePropertyOfCollectionList"]?.ToString();

                    var y = JObject.Parse(item)["Latitude"]?.ToString();
                    var y2 = JObject.Parse(item)["latitude"]?.ToString();
                    if (!String.IsNullOrEmpty(x) || !String.IsNullOrEmpty(x2))
                    {
                        var newFilter = JsonConvert.DeserializeObject<EntityMultilpleConditionsFilter>(item.Replace("\n", ""));
                        dynamicList.Add(newFilter);
                    }
                    else if (!String.IsNullOrEmpty(y) || !String.IsNullOrEmpty(y2))
                    {
                        var newFilter = JsonConvert.DeserializeObject<EntityGeometryFilter>(item.Replace("\n", ""));
                        dynamicList.Add(newFilter);
                    }
                    else
                    {
                        var newFilter = JsonConvert.DeserializeObject<EntityFilter>(item.Replace("\n", ""));
                        dynamicList.Add(newFilter);
                    }
                }
                result.DynamicFilters = dynamicList;
            }

            if (valueproviders.GetValue("IncludeProperties").Length > 0)
            {
                var IncludeProperties = new List<ColumnBase>();
                foreach (var item in valueproviders.GetValue("IncludeProperties").ToList())
                {
                    //var newIcludeProp = JsonSerializer.Deserialize<ColumnBase>(item.Replace("\n", ""), options);
                    var newIcludeProp = JsonConvert.DeserializeObject<ColumnBase>(item.Replace("\n", ""));
                    IncludeProperties.Add(newIcludeProp);
                }
                result.IncludeProperties = IncludeProperties;
            }

            bindingContext.Result = ModelBindingResult.Success(result);

            //if (valueproviders.GetValue("Id").Length > 0)
            //    result.Id = new Guid(valueproviders.GetValue("Id").FirstValue);
            //if (valueproviders.GetValue("Email").Length > 0)
            //    result.Email = valueproviders.GetValue("Email").FirstValue;
            //if (valueproviders.GetValue("Select").Length > 0)
            //    result.Select = valueproviders.GetValue("Select").ToList();
            //if (valueproviders.GetValue("Include").Length > 0)
            //    result.Include = valueproviders.GetValue("Include").ToList();

            //var newpaging = new PagingParam();
            //if (valueproviders.GetValue("Paging.Skip").Length > 0)
            //    newpaging.Skip = Convert.ToInt32(valueproviders.GetValue("Paging.Skip").FirstValue);
            //if (valueproviders.GetValue("Paging.Take").Length > 0)
            //    newpaging.Take = Convert.ToInt32(valueproviders.GetValue("Paging.Take").FirstValue);

            //result.Paging = newpaging;

            return Task.CompletedTask;
        }
    }
}
