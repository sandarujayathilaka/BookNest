using Ecommerce.DataAccess;
using Ecommerce.Models;
using MongoDB.Driver;

namespace Ecommerce.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDbContext context)
        {
            _products = context.GetCollection<Product>("Products");

            // Create unique indexes for ProductID during repository initialization
            CreateUniqueIndexes();
        }

        // This method creates the unique "constraint-like" behavior
        private void CreateUniqueIndexes()
        {
            var indexOptions = new CreateIndexOptions { Unique = true };

            // Create unique index for ProductID (mimics a constraint)
            var productIDIndex = Builders<Product>.IndexKeys.Ascending(p => p.ProductID);
            var productIDModel = new CreateIndexModel<Product>(productIDIndex, indexOptions);

            // Apply the indexes to the collection (acts like a constraint)
            _products.Indexes.CreateMany(new[] { productIDModel });
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _products.Find(product => product.IsActive).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;

            await _products.InsertOneAsync(product);
        }

        public async Task UpdateProduct(Product product)
        {
            product.UpdatedAt = DateTime.Now;

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
        public async Task<List<Product>> GetProductsByVendorId(string vendorId)
        {
            return await _products.Find(p => p.VendorID == vendorId).ToListAsync();
        }
    }
}
