using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;

namespace Ecommerce.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateNewProduct(Product product)
        {
            product.Id = ObjectId.GenerateNewId().ToString();
            product.ProductID = GenerateProductId(product.VendorID);

            await _productRepository.CreateProduct(product);
        }

        private string GenerateProductId(string vendorId)
        {
            var prefix = "PROD";
            // Get the last 5 characters from the vendorId
            var lastFive = vendorId.Substring(vendorId.Length - 5);

            var randomNumber = new Random().Next(10000, 99999);

            return $"{prefix}{lastFive}{randomNumber}";
        }


        public async Task<List<Product>> GetActiveProducts()
        {

            return await _productRepository.GetAllProducts();
        }

        public async Task UpdateProduct(Product product)
        {
            Console.WriteLine(product.Id);
            await _productRepository.UpdateProduct(product);
        }
    }
}
