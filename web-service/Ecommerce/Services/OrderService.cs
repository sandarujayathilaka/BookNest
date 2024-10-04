using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using NanoidDotNet;

namespace Ecommerce.Services
{
    public class OrderService
    {

        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrder(OrderDto orderDto)
        {
            var order = new Order
            {
                OrderID = GenerateNanoId(), 
                CustomerID = orderDto.CustomerID,
                OrderDate = orderDto.OrderDate,
                Status = orderDto.Status,
                LastStatusChange = DateTime.UtcNow,
                Products = new List<ProductOrder>(),
                TotalItems = 0,
                TotalAmount = 0m 
            };

            foreach (var product in orderDto.Products)
            {
                var productOrder = new ProductOrder
                {
                    ProductID = product.ProductID,
                    VendorID = product.VendorID,
                    Status = product.Status,
                    TotalItems = product.TotalItems,
                    UnitPrice = product.UnitPrice,
                    TotalAmount = product.TotalItems * product.UnitPrice
                };

             
                order.TotalItems += productOrder.TotalItems;
                order.TotalAmount += productOrder.TotalAmount;

                order.Products.Add(productOrder);
            }

            await _orderRepository.CreateOrder(order);
            return order;
        }


        public async Task UpdateOrderStatus(string orderId, string status)
        {
            await _orderRepository.UpdateOrderStatus(orderId, status);
        }

        public async Task<List<VendorProductDto>> GetProductsByVendorId(string vendorId)
        {
            return await _orderRepository.GetOrdersByVendorId(vendorId);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public string GenerateNanoId()
        {
            return Nanoid.Generate(Nanoid.Alphabets.LowercaseLettersAndDigits, 10);
        }

        public async Task<bool> RequestOrderCancellationAsync(string orderId, string cancelationNote)
        {
           
            return await _orderRepository.CancelOrderAsync(orderId, cancelationNote);
        }

        public async Task<bool>  OrderOfficeCancellation(string orderId,string Status, string cancelationOfficeNote)
        {
             
            return await _orderRepository.CancelOrderbyOfficer(orderId, Status, cancelationOfficeNote);
        }
        public async Task<List<Order>> GetCanceledOrders()
        {
            return await _orderRepository.GetCanceledOrders();
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAndStatus(string customerId)
        {
           
            return await _orderRepository.GetOrdersByCustomerIdAndStatus(customerId);
        }

        public async Task<List<Order>> GetCurrentOrdersByCustomer(string customerId)
        {

            return await _orderRepository.GetCurrentOrdersByCustomer(customerId);
        }
    }
}
