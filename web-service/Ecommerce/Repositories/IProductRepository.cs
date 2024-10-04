using Ecommerce.Models;
using MongoDB.Driver;

namespace Ecommerce.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(string productId);
        Task<Product> GetProductByProductId(string productId);
        Task<List<Product>> GetProductsByVendorId(string vendorId);

        Task<Product> FindOneAndUpdateAsync(FilterDefinition<Product> filter, UpdateDefinition<Product> update);
    }
}
