using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApiLabor.Bll.Exceptions;
using WebApiLabor.DAL;
using WebApiLabor.Entities;

namespace WebApiLabor.Bll.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;

        public ProductService(NorthwindContext context)
        {
            _context = context;
        }

        public Product GetProduct(int productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductOrders)
                    .ThenInclude(po => po.Order)
                .SingleOrDefault(p => p.Id == productId) ?? throw new EntityNotFoundException("Nem található a termék");
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
            _context.SaveChanges();           
        }

        public void DeleteProduct(int productId)
        {
            _context.Products.Remove(new Product { Id = productId });
            _context.SaveChanges();            
        }
    }
}
