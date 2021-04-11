using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiLab.BLL.DTO;
using WebApiLab.DAL;

namespace WebApiLab.BLL
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;

        public ProductService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteProductAsync(int productId)
        {
            _context.Products.Remove(new DAL.Entities.Product { Id = productId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.SingleOrDefault(p => p.Id == productId) == null)
                    throw new EntityNotFoundException("Nem található a termék");
                else throw;
            }
        }

        public async Task<Product> GetProductAsync(int productId)
        {
           return await _mapper.ProjectTo<Product>(
                    _context.Products.Where(p=>p.Id==productId)
                ).SingleOrDefaultAsync()
                ?? throw new EntityNotFoundException("Nem található a termék"); 
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await
                _mapper.ProjectTo<Product>(_context.Products)
                .ToListAsync();
        }

        public async Task<Product> InsertProductAsync(Product newProduct)
        {
            var efProduct = _mapper.Map<DAL.Entities.Product>(newProduct);
            _context.Products.Add(efProduct);
            await _context.SaveChangesAsync();
            return await GetProductAsync(efProduct.Id);
        }

        public async Task UpdateProductAsync(int productId, Product updatedProduct)
        {
            var efProduct = _mapper.Map<DAL.Entities.Product>(updatedProduct);
            efProduct.Id = productId;
            var entry = _context.Attach(efProduct);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Products
                        .SingleOrDefaultAsync(p => p.Id == productId) == null)
                    throw new EntityNotFoundException("Nem található a termék");
                else throw;
            }
        }
    }
}
