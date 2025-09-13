using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.SqlDynamicLinker.Enums;
using AlJawad.SqlDynamicLinker.Extensions;
using AlJawad.SqlDynamicLinker.Models;
using AlJawad.SqlDynamicLinkerShowCases.Repositories;
using AutoMapper;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AlJawad.SqlDynamicLinkerShowCases.Controllers
{
    public class HomeController : Controller
    {

        private readonly ProductRepository _repository;

        private readonly IMapper _mapper;


        /// <summary>
        /// We just including few properties to search by name
        /// </summary>

        List<ColumnBase> SearchPropertiesForSearchByName = [
            new ColumnBase("Name"),
            new ColumnBase("Description"),
            new ColumnBase("MainCategory.Name"),
        ];

        /// <summary>
        /// In our sample it does not matter while we are using mocked data
        /// But in real scenario you have to included needed sub modules and collections such MainCategory
        /// </summary>
        List<ColumnBase> IncludePropertiesForListing = [
            new ColumnBase("MainCategory")
        ];

        public HomeController(IMapper mapper, ProductRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }


        protected List<EntityFilter> BuildSearchFilter(string term)
        {
            List<EntityFilter> DynamicFilters = new List<EntityFilter>();
            if (string.IsNullOrWhiteSpace(term))
            {
                return DynamicFilters;
            }
            foreach (var column in SearchPropertiesForSearchByName)
            {
                DynamicFilters.Add(new EntityFilter()
                {
                    DataName = column.DataName,
                    Value = term,
                    Operator = EntityFilterOperators.Contains,
                    Logic = EntityFilterLogic.Or,
                });
            }
            return DynamicFilters;
        }

        protected IEnumerable<EntityBaseFilter> MergeSearchAndDefaultFilter(IEnumerable<EntityBaseFilter> DynamicFilters, String? SearchQuery)
        {
            if (SearchQuery == null && String.IsNullOrEmpty(SearchQuery))
            {
                return DynamicFilters;
            }
            var SearchFilters = BuildSearchFilter(SearchQuery);
            if (SearchFilters !=null && SearchFilters.IsNullOrEmpty())
            {
                return DynamicFilters;
            }
            if (DynamicFilters.IsNullOrEmpty())
            {
                return SearchFilters;
            }

            return new List<EntityFilter>()
            {
                new EntityFilter(){
                Filters = DynamicFilters
            },
                new EntityFilter(){
                Logic = EntityFilterLogic.And,
                Filters = SearchFilters
            }
            };

        }


        public virtual async Task<IActionResult> Get([DataTablesRequest] DataTablesRequest requestModel)
        {
            try
            {
                var filter = _mapper.Map<BasePagingFilter>(requestModel);

                var QueryAndSearchFilter = MergeSearchAndDefaultFilter(filter.DynamicFilters, requestModel.Search?.Value);

                filter.DynamicFilters = QueryAndSearchFilter;

                filter.IncludeProperties = IncludePropertiesForListing;

                var response = _repository.GetProducts();

                response = response.Filter(filter);
                response = response.Sort(filter.DynamicSorting);

                return Json(new
                {
                    draw = requestModel.Draw,
                    recordsFiltered = response.Count(),// response.PageCount,
                    recordsTotal = response.Count(),
                    data = response,
                    length = requestModel.Length,
                    pageLength = requestModel.Length,
                    //start = 1
                });
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
                msg += "a";
                return Json(msg);
            }
        }

    }
}
