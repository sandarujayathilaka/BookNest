using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

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

            await _productRepository.CreateProduct(product);
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

        public async Task<Product> UpdateProductStatusAsync(string productId, string status, string deniedMessage)
        {
            // Retrieve the product
            var product = (await _productRepository.GetAllProducts()).FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                product.Status = status;
                product.DeniedMessage = deniedMessage;

                // Call the repository update method
                await _productRepository.UpdateProduct(product);
                return product;
            }

            return null;
        }


    }
}
