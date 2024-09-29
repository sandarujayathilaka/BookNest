using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecommerce.Repositories;
using Ecommerce.Services;
using Ecommerce.Dto;

namespace EcommercePlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductService _productService;

        public ProductController(IProductRepository productRepository,ProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;

        }

        // Get all active products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetActiveProducts();
            return Ok(products);
        }

        // Vendor: Create a new product
        [HttpPost]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var product = new Product
            {
                ProductID = productDto.ProductID,
                Description = productDto.Description,
                Name = productDto.Name,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                IsActive=productDto.IsActive,
                VendorID = productDto.VendorID
            };

            await _productService.CreateNewProduct(product);
            return Ok("Product created.");
        }

        // Vendor: Update product
        [HttpPut("{productId}")]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> UpdateProduct(string productId, [FromBody] ProductDto productDto)
        {
            var existingProduct = await _productRepository.GetProductByProductId(productId);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            existingProduct.Name = productDto.Name ?? existingProduct.Name;
            existingProduct.Description = productDto.Description ?? existingProduct.Description;
            existingProduct.Price = productDto.Price != default ? productDto.Price : existingProduct.Price;
            existingProduct.StockQuantity = productDto.StockQuantity != default ? productDto.StockQuantity : existingProduct.StockQuantity;
            existingProduct.IsActive = productDto.IsActive;

            await _productService.UpdateProduct(existingProduct);
            return Ok("Product updated.");
        }

        // Vendor: Delete product
        [HttpDelete("{productId}")]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            await _productRepository.DeleteProduct(productId);
            return Ok("Product deleted.");
        }
    }
}
