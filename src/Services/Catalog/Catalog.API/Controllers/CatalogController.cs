using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController :ControllerBase
    {
        private readonly IProductRepository _reporsitory;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository reporsitory,ILogger<CatalogController> logger)
        {
            _reporsitory = reporsitory;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _reporsitory.GetProducts();
            return Ok(products);
        }
        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _reporsitory.GetProduct(id);
            if(product == null)
            {
                _logger.LogError($"Product with id:{id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }


        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductByCategory(string category)
        {
            var product = await _reporsitory.GetProductByCategory(category);
            if (product == null)
            {
                _logger.LogError($"Product with category:{category}, not found.");
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await _reporsitory.CreateProduct(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _reporsitory.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}",Name ="DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _reporsitory.DeleteProduct(id));
        }
    }
}
