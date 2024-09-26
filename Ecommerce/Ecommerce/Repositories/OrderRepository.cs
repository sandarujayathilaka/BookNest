using Ecommerce.DataAccess;
using Ecommerce.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Repositories
{
    public class OrderRepository : IOrderRepository // Implement the IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(MongoDbContext context)
        {
            _orders = context.GetCollection<Order>("Orders");
        }

        // Create a new order
        public async Task CreateOrder(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        // Get orders by customer ID
        public async Task<List<Order>> GetOrdersByCustomerId(string customerId)
        {
            // Query to find orders by customer ID
            return await _orders.Find(order => order.CustomerID == customerId).ToListAsync();
        }

        // Update the status of an order
        public async Task UpdateOrderStatus(string orderId, string status)
        {
            Console.WriteLine("wwwwwwwwwwwwwwwwwwwww");
            // Filter to find the order by its ID
            var filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderId);
            Console.WriteLine("hidf###############");
            // Update to set the new status
            var update = Builders<Order>.Update.Set(o => o.Status, status);
            // Perform the update operation
            await _orders.UpdateOneAsync(filter, update);
        }
    }
}
