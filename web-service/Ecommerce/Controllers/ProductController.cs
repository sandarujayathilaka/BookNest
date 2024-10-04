using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories;
using Ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommercePlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductService _productService;

        public ProductController(IProductRepository productRepository, ProductService productService)
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

        // Get a product by ID
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(string productId)
        {
            var product = await _productRepository.GetProductByProductId(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }

        // Vendor: Create a new product
        [HttpPost]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {

            // Extract UserId (VendorId) from JWT claims using ClaimTypes.NameIdentifier
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var product = new Product
            {
                Title = productDto.Title,
                Author = productDto.Author,
                ISBN = productDto.ISBN,
                Category = productDto.Category,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                Image = productDto.Image,
                VendorID = userIdFromToken!,
                IsActive = false,
                Status = "Pending",
            };

            await _productService.CreateNewProduct(product);
            return Ok("Product created.");
        }

        // Vendor: Update product
        [HttpPut("{productId}")]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> UpdateProduct(string productId, [FromBody] CreateProductDto productDto)
        {
            var existingProduct = await _productRepository.GetProductByProductId(productId);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            // Extract UserId (VendorId) from JWT claims using ClaimTypes.NameIdentifier
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the product's VendorID matches the logged-in user's UserId (acting as VendorID)
            if (existingProduct.VendorID.ToString() != userIdFromToken)
            {
                return BadRequest("You do not have permission to update this product.");
            }

            existingProduct.Title = productDto.Title ?? existingProduct.Title;
            existingProduct.Author = productDto.Author ?? existingProduct.Author;
            existingProduct.ISBN = productDto.ISBN ?? existingProduct.ISBN;
            existingProduct.Category = productDto.Category ?? existingProduct.Category;
            existingProduct.Description = productDto.Description ?? existingProduct.Description;
            existingProduct.Price = productDto.Price != default ? productDto.Price : existingProduct.Price;
            existingProduct.StockQuantity = productDto.StockQuantity != default ? productDto.StockQuantity : existingProduct.StockQuantity;
            existingProduct.Image = productDto.Image ?? existingProduct.Image;
            existingProduct.IsActive = productDto.IsActive;


            await _productService.UpdateProduct(existingProduct);
            return Ok("Product updated.");
        }

        // Vendor: Delete product
        [HttpDelete("{productId}")]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            // Retrieve the existing product
            var existingProduct = await _productRepository.GetProductByProductId(productId);

            // Check if the product exists
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            // Extract UserId (VendorId) from JWT claims using ClaimTypes.NameIdentifier
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the product's VendorID matches the logged-in user's UserId (acting as VendorID)
            if (existingProduct.VendorID.ToString() != userIdFromToken)
            {
                return BadRequest("You do not have permission to delete this product.");
            }


            // Proceed to delete the product
            await _productRepository.DeleteProduct(productId);
            return Ok("Product deleted.");
        }

        // PUT: api/Product/updateStatus
        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateProductStatus([FromQuery] string productID, [FromQuery] string status, [FromBody] string deniedMessage = null)
        {
            if (string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(status))
            {
                Console.WriteLine("Invalid product data");
                return BadRequest("Product ID and status are required.");
            }

            try
            {
                // Handle product approval or denial
                string finalStatus = status.ToLower() == "denied" && !string.IsNullOrEmpty(deniedMessage)
                    ? "Denied"
                    : status;

                // Update product status and set deniedMessage only if the status is "Denied"
                var updatedProduct = await _productService.UpdateProductStatusAsync(productID, finalStatus, finalStatus == "Denied" ? deniedMessage : null);

                if (updatedProduct == null)
                {
                    Console.WriteLine("Product not found.");
                    return NotFound("Product not found.");
                }

                Console.WriteLine("Product status updated successfully.");
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product status: {ex.Message}");  // Log the exception
                return StatusCode(500, "Internal server error.");
            }
        }



       
        // Vendor: Get vendor's products
        [HttpGet("vendor/products")]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> GetVendorProducts()
        {
            // Extract VendorID (UserId from the JWT token)
            var vendorIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(vendorIdFromToken))
            {
                return BadRequest("Unable to retrieve vendor information.");
            }

            // Fetch products associated with the vendor
            var vendorProducts = await _productRepository.GetProductsByVendorId(vendorIdFromToken);

            return Ok(vendorProducts);
        }

    }
}
