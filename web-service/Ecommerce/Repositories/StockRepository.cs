using Ecommerce.DataAccess;
using Ecommerce.Models;
using MongoDB.Driver;

namespace Ecommerce.Repositories
{
    public class StockRepository
    {
        private readonly IMongoCollection<Stock> _stocks;

        public StockRepository(MongoDbContext context)
        {
            _stocks = context.GetCollection<Stock>("Stocks");
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _stocks.Find(stock => true).ToListAsync();
        }

        public async Task<Stock> GetStockByProductIdAsync(string productId)
        {
            return await _stocks.Find(stock => stock.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<Stock> GetStockByIdAsync(string id)
        {
            return await _stocks.Find(stock => stock.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddStockAsync(Stock stock)
        {
            await _stocks.InsertOneAsync(stock);
        }

        public async Task UpdateStockAsync(string id, Stock stock)
        {
            await _stocks.ReplaceOneAsync(s => s.Id == id, stock);
        }

        public async Task UpdateLowStockAlertStatusAsync(string productId, bool isLowStockAlert)
        {
            var filter = Builders<Stock>.Filter.Eq(stock => stock.ProductId, productId);
            var update = Builders<Stock>.Update.Set(stock => stock.IsLowStockAlert, isLowStockAlert);
            await _stocks.UpdateOneAsync(filter, update);
        }

        public async Task RemoveStockAsync(string id)
        {
            await _stocks.DeleteOneAsync(s => s.Id == id);
        }
    }

}
