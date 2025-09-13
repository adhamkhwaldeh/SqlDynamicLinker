using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.SqlDynamicLinkerShowCases.Entities;
using AlJawad.SqlDynamicLinkerShowCases.Repositories;
using Microsoft.AspNetCore.Http;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using DataTables.AspNetCore.Mvc.Binder;
using AlJawad.SqlDynamicLinker.Enums;
using AlJawad.SqlDynamicLinker.Models;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using AlJawad.SqlDynamicLinker.Extensions;
using Newtonsoft.Json;
using AlJawad.SqlDynamicLinker.Converters;

namespace AlJawad.SqlDynamicLinkerShowCases.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly ProductRepository _repository;

        private readonly IMapper _mapper;
        
        public ProductAPIController(IMapper mapper,ProductRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/productapi/products
        [HttpGet("products")]
        public IActionResult GetAllProducts([FromQuery] BaseQueryableFilter baseFilter)
        {
            var products = _repository.GetProducts().Filter(baseFilter);
            return Ok(products);
        }

        // GET: api/productapi/categories
        [HttpGet("categories")]
        public IActionResult GetAllCategories()
        {
            var categories = _repository.GetCategories();
            return Ok(categories);
        }

        // GET: api/producttest/filter-from-file/{fileName}
        [HttpGet("filter-from-file/{fileName}")]
        public IActionResult GetProductsFromFile(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Samples", fileName + ".json");
            if (!System.IO.File.Exists(path))
            {
                return NotFound($"Sample file not found: {fileName}");
            }

            var json = System.IO.File.ReadAllText(path);
            var settings = new JsonSerializerSettings
            {
                Converters = { new EntityBaseFilterConverter() },
                NullValueHandling = NullValueHandling.Ignore
            };

            var filter = JsonConvert.DeserializeObject<BaseQueryableFilter>(json, settings);

            var products = _repository.GetProducts().Filter(filter);
            return Ok(products);
        }
    }
}