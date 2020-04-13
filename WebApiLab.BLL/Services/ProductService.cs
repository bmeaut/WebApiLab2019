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

        public void DeleteProduct(int productId)
        {
            _context.Products.Remove(new Product { Id = productId });          
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.Find(productId) == null)
                    throw new EntityNotFoundException("Nem található a termék");
                else throw;
            }
        }

        public Product GetProduct(int productId)
        {
            return _context.Products
               .Include(p => p.Category)
               .Include(p => p.ProductOrders)
                   .ThenInclude(po => po.Order)
               .SingleOrDefault(p => p.Id == productId) 
               ?? throw new EntityNotFoundException("Nem található a termék");
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductOrders)
                    .ThenInclude(po => po.Order)
                .ToList();

            return products;
        }

        public Product InsertProduct(Product newProduct)
        {
            _context.Products.Add(newProduct);
            _context.SaveChanges();
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
