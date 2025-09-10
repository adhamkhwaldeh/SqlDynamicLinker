using AlJawad.SqlDynamicLinker.DynamicFilter;
using AlJawad.SqlDynamicLinkerShowCases.Entities;
using AlJawad.SqlDynamicLinkerShowCases.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AlJawad.SqlDynamicLinker.DynamicFilter;

namespace AlJawad.SqlDynamicLinkerShowCases.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly ProductRepository _repository;

        // Inject the singleton repository via constructor
        public ProductAPIController(ProductRepository repository)
        {
            _repository = repository;
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


    }
}