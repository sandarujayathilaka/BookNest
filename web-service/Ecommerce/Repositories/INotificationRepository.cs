using Ecommerce.Models;
using MongoDB.Driver;

namespace Ecommerce.Repositories
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsForUserAsync(string userId);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<Notification> FindOneAsync(FilterDefinition<Notification> filter);
        Task<UpdateResult> UpdateOneAsync(FilterDefinition<Notification> filter, UpdateDefinition<Notification> update);
    }
}
