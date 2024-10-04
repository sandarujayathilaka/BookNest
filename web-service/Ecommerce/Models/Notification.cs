using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.Models
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Unique identifier
        public string Message { get; set; } // Notification content
        public string UserId { get; set; } // Nullable. Null means the notification is for all users
        public DateTime Timestamp { get; set; } // Time the notification was created
        public bool IsRead { get; set; }
        public List<string> ReadBy { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"Id: {Id}, UserId: {UserId}, Message: {Message}, Timestamp: {Timestamp}, IsRead: {IsRead}, ReadBy: {string.Join(", ", ReadBy ?? new List<string>())}";
        }
    }
}
