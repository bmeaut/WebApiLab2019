using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiLabor.Bll.Services;
using WebApiLabor.Entities;

namespace WebApiLabor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
       


        // GET: api/Products
        //[HttpGet]
        //public IEnumerable<Product> Get()
        //{
        //    return _productService.GetProducts();
        //}

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return _productService.GetProducts()
                .ToList();
        }       

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Product> Get(int id)
        {
            return _productService.GetProduct(id);
        }

        // POST: api/Products
        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            var created = _productService.InsertProduct(product);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            _productService.UpdateProduct(id, product);
            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
