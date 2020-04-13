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
        public ActionResult<IEnumerable<Product>> Get()
        {
            return _mapper.Map<List<Product>>(_productService.GetProducts());
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

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Product> Get(int id)
        {
            return _mapper.Map<Product>(_productService.GetProduct(id));            
        }

        // POST: api/Products
        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            var created = _productService
                .InsertProduct(_mapper.Map<Entities.Product>(product));
            return CreatedAtAction(
                        nameof(Get),
                        new { id = created.Id },
                        _mapper.Map<Product>(created)
            );
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            //_productService.
            //    UpdateProductAsync(id, _mapper.Map<Entities.Product>(product));
            await _productService.UpdateProductAsync(id, _mapper.Map<Entities.Product>(product));
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
