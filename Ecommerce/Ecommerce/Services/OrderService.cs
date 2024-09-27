using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;

namespace Ecommerce.Services
{
    public class OrderService
    {

        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task CreateNewOrder(Order order)
        {
           order.OrderID = ObjectId.GenerateNewId().ToString();

           await _orderRepository.CreateOrder(order);
        }

        public async Task UpdateOrderStatus(string orderId, string status)
        {
            await _orderRepository.UpdateOrderStatus(orderId, status);
        }

    }
}
