using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void DeleteProduct(int productId)
        {
            _context.Products.Remove(new DAL.Entities.Product { Id = productId });
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.SingleOrDefault(p => p.Id == productId) == null)
                    throw new EntityNotFoundException("Nem található a termék");
                else throw;
            }
        }

        public Product GetProduct(int productId)
        {
           return _mapper.ProjectTo<Product>(_context.Products.Where(p=>p.Id==productId))
               .SingleOrDefault() ?? throw new EntityNotFoundException("Nem található a termék"); 
        }

        public IEnumerable<Product> GetProducts()
        {
            // var xx = _context.Products
            //.Include(p => p.Category)
            //.Include(p => p.ProductOrders)
            //    .ThenInclude(po => po.Order).ToArray();
            // var products = _mapper.Map<IEnumerable<Product>>(xx);
            var products =
                _mapper.ProjectTo<Product>(_context.Products)
                .AsEnumerable();

            return products;
        }

        public Product InsertProduct(Product newProduct)
        {
            var efProduct = _mapper.Map<DAL.Entities.Product>(newProduct);
            _context.Products.Add(efProduct);
            _context.SaveChanges();
            return GetProduct(efProduct.Id);
        }

        public void UpdateProduct(int productId, Product updatedProduct)
        {
            var efProduct = _mapper.Map<DAL.Entities.Product>(updatedProduct);
            efProduct.Id = productId;
            var entry = _context.Attach(efProduct);
            entry.State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Products.SingleOrDefault(p => p.Id == productId) == null)
                    throw new EntityNotFoundException("Nem található a termék");
                else throw;
            }
        }
        /*Többi függvény generált implementációja*/
    }
}
