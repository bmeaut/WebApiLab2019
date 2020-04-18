using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLab.Entities;

namespace WebApiLab.BLL
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int productId);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> InsertProductAsync(Product newProduct);
        Task UpdateProductAsync(int productId, Product updatedProduct);
        Task DeleteProductAsync(int productId);
    }
}
