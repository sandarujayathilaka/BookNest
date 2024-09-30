using Ecommerce.DataAccess;
using Ecommerce.Models;
using MongoDB.Driver;

namespace Ecommerce.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMongoCollection<Notification> _notifications;

        public NotificationRepository(MongoDbContext context)
        {
            _notifications = context.Notifications;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _notifications.InsertOneAsync(notification);
        }

        public async Task<List<Notification>> GetNotificationsForUserAsync(string userId)
        {
            var filter = Builders<Notification>.Filter.Or(
                Builders<Notification>.Filter.Eq(n => n.UserId, null),
                Builders<Notification>.Filter.Eq(n => n.UserId, userId)
            );

            return await _notifications.Find(filter).SortByDescending(n => n.Timestamp).ToListAsync();
        }
        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _notifications.Find(_ => true).SortByDescending(n => n.Timestamp).ToListAsync();
        }
        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<Notification> filter, UpdateDefinition<Notification> update)
        {
            return await _notifications.UpdateOneAsync(filter, update);
        }

        public async Task<Notification> FindOneAsync(FilterDefinition<Notification> filter)
        {
            // Use FindAsync to get the first notification matching the filter
            return await _notifications.Find(filter).FirstOrDefaultAsync();
        }
        //public async Task<List<Notification>> GetAllNotificationsAsync()
        //{
        //    var filter = Builders<Notification>.Filter.Eq(n => n.UserId, userId);
        //    return await _notifications.Find(filter).SortByDescending(n => n.Timestamp).ToListAsync();
        //}

    }
}
