using Ecommerce.Models;


namespace Ecommerce.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrder(Order order);

        Task<List<Order>> GetOrdersByCustomerId(string customerId);

        Task UpdateOrderStatus(string orderId, string status);

       
    }
}
