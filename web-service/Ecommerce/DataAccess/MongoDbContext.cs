using Ecommerce.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ecommerce.DataAccess
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);

            Console.WriteLine($"Connected to database: {settings.Value.DatabaseName}");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Stock> Stocks => _database.GetCollection<Stock>("Stocks");

        public IMongoCollection<Notification> Notifications => _database.GetCollection<Notification>("Notifications");


    }

}
