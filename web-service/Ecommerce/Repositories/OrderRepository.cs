using Ecommerce.DataAccess;
using Ecommerce.Dto;
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

        public async Task<List<VendorProductDto>> GetOrdersByVendorId(string vendorId)
        {
            var orders = await _orders.Find(order => order.Products.Any(product => product.VendorID == vendorId)).ToListAsync();

            var vendorProducts = orders.SelectMany(order => order.Products
                .Where(product => product.VendorID == vendorId)
                .Select(product => new VendorProductDto
                {
                    OrderID = order.OrderID,
                    CustomerID = order.CustomerID,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    Product = product
                }))
                .ToList();

            return vendorProducts;
        }



        public async Task<Order> GetOrderByOrderId(string orderId)
        {
            return await _orders.Find(p => p.OrderID == orderId).FirstOrDefaultAsync();
        }

        public async Task UpdateOrderStatus(string orderId, string status)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderId);

            var update = Builders<Order>.Update
                .Set(o => o.Status, status)
                .Set(o => o.LastStatusChange, DateTime.UtcNow); 
            
            await _orders.UpdateOneAsync(filter, update);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _orders.Find(order => true).ToListAsync();
        }

        public async Task<bool> CancelOrderAsync(string orderId, string cancelationNote)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderId);
            var update = Builders<Order>.Update
                .Set(o => o.OrderCancelation, true)
                .Set(o => o.cancelationNote, cancelationNote);

            var result = await _orders.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        public async Task<List<Order>> GetCanceledOrders()
        {
          
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Eq(o => o.OrderCancelation, true),
                Builders<Order>.Filter.Ne(o => o.Status, "Canceled") 
            );

            return await _orders.Find(filter).ToListAsync();
        }


        public async Task<bool> CancelOrderbyOfficer(string orderId, string status, string cancelationOfficeNote)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderId);
        var update = Builders<Order>.Update
                .Set(o => o.Status, status)  
                .Set(o => o.CancelationOfficerNote, cancelationOfficeNote)  
                .Set(o => o.LastStatusChange, DateTime.UtcNow);  

            var result = await _orders.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAndStatus(string customerId)
        {
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Eq(order => order.CustomerID, customerId),
                Builders<Order>.Filter.In(order => order.Status, new[] { "Canceled", "Delivered" })
            );

            return await _orders.Find(filter).ToListAsync();
        }

        public async Task<List<Order>> GetCurrentOrdersByCustomer(string customerId)
        {
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Eq(order => order.CustomerID, customerId),
                Builders<Order>.Filter.In(order => order.Status, new[] { "Pending", "Dispatched" })
            );

            return await _orders.Find(filter).ToListAsync();
        }

    }
}
