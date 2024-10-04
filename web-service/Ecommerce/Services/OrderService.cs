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

        public async Task<object> GetVendorInfoFromOrders(string vendorId)
        {
            Console.WriteLine($"service {vendorId}");
            // Fetch vendor orders using the new method
            var orders = await _orderRepository.GetVendorOrdersWithDetails(vendorId);
            Console.WriteLine($"service order {orders}");
            // Variables to calculate success and failure rates
            int totalOrders = 0;
            int successfulDeliveries = 0;
            int failedDeliveries = 0;
            int pendingOrders = 0;
            var reasonsForFailure = new List<string>();

            foreach (var order in orders)
            {
                foreach (var product in order.Products)
                {
                    Console.WriteLine($"service order {product.VendorID}");
                    if (product.VendorID == vendorId)
                    {
                        Console.WriteLine($"service order {orders}");
                        totalOrders++;

                        if (product.Status == "Delivered")
                        {
                            successfulDeliveries++;
                        }
                        else if (product.Status == "Cancelled")
                        {
                            failedDeliveries++;
                            reasonsForFailure.Add($"Order {order.OrderID} failed due to {product.Status}");
                        }
                        else if (product.Status == "Pending")
                        {
                            pendingOrders++;
                        }
                    }
                }
            }

            // Calculate success and failure rates
            decimal successRate = totalOrders > 0 ? (successfulDeliveries / (decimal)totalOrders) * 100 : 0;
            decimal unsuccessRate = totalOrders > 0 ? (failedDeliveries / (decimal)totalOrders) * 100 : 0;
            decimal pendingRate = totalOrders > 0 ? (pendingOrders / (decimal)totalOrders) * 100 : 0;

            // Constructing the response object
            var vendorInfo = new
            {
                VendorId = vendorId,
                SuccessRate = successRate,
                UnsuccessRate = unsuccessRate,
                PendingRate = pendingRate,
                ReasonsForUnsuccessfulDeliveries = reasonsForFailure
            };

            return vendorInfo;
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
