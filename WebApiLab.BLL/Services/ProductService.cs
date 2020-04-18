using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLab.DAL;
using WebApiLab.Entities;

namespace WebApiLab.BLL
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;

        public ProductService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task DeleteProductAsync(int productId)
        {
            _context.Products.Remove(new Product { Id = productId });          
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.Products
                    .SingleOrDefaultAsync(p=>p.Id == productId)) == null)
                    throw new EntityNotFoundException("Nem található a termék");
                else throw;
            }
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return (await _context.Products
               .Include(p => p.Category)
               .Include(p => p.ProductOrders)
                   .ThenInclude(po => po.Order)
               .SingleOrDefaultAsync(p => p.Id == productId))
               ?? throw new EntityNotFoundException("Nem található a termék");
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductOrders)
                    .ThenInclude(po => po.Order)
                .ToListAsync();

            return products;
        }

        public async Task<Product> InsertProductAsync(Product newProduct)
        {
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public void UpdateProduct(int productId, Product updatedProduct)
        {
            updatedProduct.Id = productId;
            var entry = _context.Attach(updatedProduct);
            entry.State = EntityState.Modified;
            try
            {
                 _context.SaveChanges(); //async változat hívása
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.SingleOrDefault(p=>p.Id == productId) == null)
                    throw new EntityNotFoundException("Nem található a termék");
                else throw;
            }
        }

        public async Task UpdateProductAsync(int productId, Product updatedProduct)
        {
            updatedProduct.Id = productId;
            var entry = _context.Attach(updatedProduct);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync(); //async változat hívása
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _context.Products
                        .SingleOrDefaultAsync(p => p.Id == productId)) == null)
                    throw new EntityNotFoundException("Nem található a termék");
                else throw;
            }
        }
    }
}
