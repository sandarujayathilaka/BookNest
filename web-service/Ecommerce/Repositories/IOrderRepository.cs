using Ecommerce.Dto;
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

        Task<List<VendorProductDto>> GetOrdersByVendorId(string vendorId);
        Task<List<Order>> GetPendingOrdersByProductIdAsync(string productId);
        Task<List<Order>> GetAllOrders();
        Task<bool> CancelOrderAsync(string orderId, string cancelationNote);
        Task<bool> CancelOrderbyOfficer(string orderId, string Status, string cancelationOfficeNote);
        Task<List<Order>> GetCanceledOrders(); 
        


        Task<List<Order>> GetVendorOrdersWithDetails(string vendorId);
    }
}
