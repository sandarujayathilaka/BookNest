using Ecommerce.Models;
using Ecommerce.DataAccess;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderService _orderService;

        public OrderController(IOrderRepository orderRepository,OrderService orderService)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
        }

        // Customer: Place an order
        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
        {
            var order = new Order
            {
                ProductID = orderDto.ProductID,
                CustomerID = orderDto.CustomerID,
                VendorID = orderDto.VendorID,
                OrderDate = orderDto.OrderDate,
                Status = orderDto.Status
            };

            await _orderService.CreateNewOrder(order);
            return Ok("Order placed.");
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
        [HttpGet("vendor/{vendorId}")]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> GetOrdersByVendor(string vendorId)
        {
            var orders = await _orderRepository.GetOrdersByVendorId(vendorId);
            return Ok(orders);
        }


        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromBody] string status)
        {
            await _orderService.UpdateOrderStatus(orderId, status);
            return Ok("Order status updated.");
        }

    }
}
