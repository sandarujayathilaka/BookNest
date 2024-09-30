using Ecommerce.Hubs;
using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;
using Microsoft.AspNetCore.SignalR;

namespace Ecommerce.Services
{
    public class StockService
    {
        private readonly StockRepository _stockRepository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationRepository _notificationRepository;
        public StockService(StockRepository stockRepository, IHubContext<NotificationHub> notificationHub, IOrderRepository orderRepository, INotificationRepository notificationRepository)
        {
            _stockRepository = stockRepository;
            _notificationHub = notificationHub;
            _orderRepository = orderRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _stockRepository.GetAllStocksAsync();
        }

        public async Task<Stock> GetStockByProductIdAsync(string productId)
        {
            return await _stockRepository.GetStockByProductIdAsync(productId);
        }


        //        public async Task AddOrUpdateStockAsync(Stock stock, bool isAdding)
        //        {
        //            try
        //            {
        //                var existingStock = await _stockRepository.GetStockByProductIdAsync(stock.ProductId);
        //            if (existingStock == null)
        //            {
        //                stock.Id = ObjectId.GenerateNewId().ToString();
        //                //stock.LowStockThreshold = 10;
        //                //stock.IsLowStockAlert = false;
        //                if (stock.Quantity < stock.LowStockThreshold)
        //                {
        //                    throw new Exception("Quantity must be more than 10"); 
        //                }
        //                await _stockRepository.AddStockAsync(stock);
        //            }
        //            else
        //            {

        //                if (isAdding)
        //                {

        //                    existingStock.Quantity += stock.Quantity;
        //                }
        //                else
        //                {

        //                    if (existingStock.Quantity - stock.Quantity < 0)
        //                    {
        //                        throw new Exception("Cannot reduce stock below zero");  
        //                    }

        //                    existingStock.Quantity -= stock.Quantity;
        //                }


        //                if (existingStock.Quantity <= existingStock.LowStockThreshold && !existingStock.IsLowStockAlert)
        //                {

        //                    await _notificationHub.Clients.User(existingStock.VendorId)
        //                        .SendAsync("ReceiveNotification", $"Product {existingStock.ProductId} is low on stock!");

        //                    existingStock.IsLowStockAlert = true;
        //                }
        //                else if (existingStock.Quantity > existingStock.LowStockThreshold)
        //                {

        //                    existingStock.IsLowStockAlert = false;
        //                }

        //                await _stockRepository.UpdateStockAsync(existingStock.Id, existingStock);
        //            }

        //        }
        //catch (Exception ex)
        //{
        //                throw new Exception($"Error updating stock: {ex.Message}", ex);
        //            }
        //}


        public async Task AddStockAsync(Stock stock)
        {
            var existingStock = await _stockRepository.GetStockByProductIdAsync(stock.ProductId);
            if (existingStock != null)
            {
                throw new Exception("Stock already exists for this product. Use the update endpoint instead.");
            }

            stock.Id = ObjectId.GenerateNewId().ToString();
            await _stockRepository.AddStockAsync(stock);
        }

        public async Task UpdateStockAsync(string productId, int quantity, bool isAdding)
        {
            var existingStock = await _stockRepository.GetStockByProductIdAsync(productId);
            if (existingStock == null)
            {
                throw new Exception("Stock not found for this product.");
            }

            // Adjust quantity based on the `isAdding` flag
            if (isAdding)
            {
                existingStock.Quantity += quantity;
                existingStock.IsLowStockAlert = false;
            }
            else
            {
                if (existingStock.Quantity - quantity < 0)
                {
                    throw new Exception("Cannot reduce stock below zero.");
                }
                existingStock.Quantity -= quantity;
            }

            // Update low stock alert status
            if (existingStock.Quantity <= existingStock.LowStockThreshold && !existingStock.IsLowStockAlert)
            {
                var message = $"Stock Alert for Product {existingStock.ProductId} is low on stock!. Current stock quantity is {existingStock.Quantity}";

                var notification = new Notification
                {
                    Message = message,
                    UserId = existingStock.UserId,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };
                Console.WriteLine($"Sending notification for product {existingStock.ProductId}");
                await _notificationRepository.AddNotificationAsync(notification);
               
              
                await _notificationHub.Clients.User(existingStock.UserId)
                    .SendAsync("ReceiveNotification", notification);

                existingStock.IsLowStockAlert = true;
            }
            else if (existingStock.Quantity > existingStock.LowStockThreshold)
            {
                existingStock.IsLowStockAlert = false;
            }

            await _stockRepository.UpdateStockAsync(existingStock.Id, existingStock);
        }

        public async Task UpdateLowStockAlertStatusAsync(string productId, bool isLowStockAlert)
        {
            var existingStock = await _stockRepository.GetStockByProductIdAsync(productId);
            if (existingStock == null)
            {
                throw new Exception("Stock not found for this product.");
            }

            existingStock.IsLowStockAlert = isLowStockAlert;
            await _stockRepository.UpdateLowStockAlertStatusAsync(productId, isLowStockAlert);
        }

        public async Task RemoveStockAsync(string id)
        {
            var stock = await _stockRepository.GetStockByIdAsync(id);

            if (stock == null)
            {
                throw new Exception("Stock entry not found.");
            }

           
            var pendingOrders = await _orderRepository.GetPendingOrdersByProductIdAsync(stock.ProductId);

           
            if (pendingOrders.Any())
            {
                throw new InvalidOperationException("Cannot remove stock for products that are part of pending orders.");
            }

            // If no pending orders, proceed with stock removal
            await _stockRepository.RemoveStockAsync(id);
        }
    }

}
