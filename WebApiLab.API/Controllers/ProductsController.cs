using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiLab.BLL;
using WebApiLab.API.DTO;

namespace WebApiLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAsync()
        {
            return _mapper.Map<List<Product>>(await _productService.GetProductsAsync());
        }

        #region Get without global exception handling
        //[HttpGet("{id}", Name = "Get")]
        //public ActionResult<Product> Get(int id)
        //{
        //    try
        //    {
        //        return _mapper.Map<Product>(_productService.GetProduct(id));
        //    }
        //    catch (EntityNotFoundException)
        //    {
        //        ProblemDetails details = new ProblemDetails
        //        {
        //            Title = "Invalid ID",
        //            Status = StatusCodes.Status404NotFound,
        //            Detail = $"No product with ID {id}"
        //        };
        //        return NotFound(details); //ProblemDetails átadása
        //    }
        //}
        #endregion

        /// <summary>
        /// Get a specific product with the given identifier
        /// </summary>
        /// <param name="id">Product's identifier</param>
        /// <returns>Returns a specific product with the given identifier</returns>
        /// <response code="200">Listing successful</response>
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            return _mapper.Map<Product>(await _productService.GetProductAsync(id));            
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>Returns the product inserted</returns>
        /// <response code="201">Insert successful</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> Post([FromBody] Product product)
        {
            var created = await _productService
                .InsertProductAsync(_mapper.Map<Entities.Product>(product));
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        _mapper.Map<Product>(created)
            );
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            await _productService.UpdateProductAsync(id, _mapper.Map<Entities.Product>(product));
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
