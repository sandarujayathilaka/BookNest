using Ecommerce.Models;
using Ecommerce.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecommerce.Repositories;
using Ecommerce.Services;
using Ecommerce.Dto;
using MongoDB.Bson;

namespace EcommercePlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderService _orderService;

        public OrderController(IOrderRepository orderRepository,OrderService orderService)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
        }


        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
        {
            var order = await _orderService.CreateOrder(orderDto);
            return Ok("Order placed successfully.");
        }



        // Customer: Get orders by customer ID
        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> GetOrdersByCustomer(string customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerId(customerId);
            return Ok(orders);
        }

        //  Get orders by Vender ID
        [HttpGet("vendor/products/{vendorId}")]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> GetProductsByVendorId(string vendorId)
        {
            var vendorProducts = await _orderService.GetProductsByVendorId(vendorId);

            if (vendorProducts == null || !vendorProducts.Any())
            {
                return NotFound("No products found for this vendor.");
            }

            return Ok(vendorProducts);
        }




        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromBody] string status)
        {
            await _orderService.UpdateOrderStatus(orderId, status);
            return Ok("Order status updated.");
        }

    }
}
