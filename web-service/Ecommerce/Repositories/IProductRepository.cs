using Ecommerce.Models;

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

    }
}
