using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrder(Order order);

        Task<List<Order>> GetOrdersByCustomerId(string customerId);

        Task<Order> GetOrderByOrderId(string orderId);

        Task UpdateOrderStatus(string orderId, string status);

        //Task<List<Order>> GetOrdersByVendorId(string vendorId);

    }
}
