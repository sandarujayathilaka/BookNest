using Ecommerce.DataAccess;
using Ecommerce.Models;
using MongoDB.Driver;

namespace Ecommerce.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDbContext context)
        {
            _products = context.GetCollection<Product>("Products");
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _products.Find(product => product.IsActive).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task UpdateProduct(Product product)
        {
            await _products.ReplaceOneAsync(p => p.ProductID == product.ProductID, product);
        }

        public async Task DeleteProduct(string productId)
        {
            await _products.DeleteOneAsync(product => product.ProductID == productId);
        }

        public async Task<Product> GetProductByProductId(string productId)
        {
            return await _products.Find(p => p.ProductID == productId).FirstOrDefaultAsync();
        }
        public async Task<Product> FindOneAndUpdateAsync(FilterDefinition<Product> filter, UpdateDefinition<Product> update)
        {
            return await _products.FindOneAndUpdateAsync(filter, update);
        }
    }
}
