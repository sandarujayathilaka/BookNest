using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ecommerce.Models
{
    public class VendorFeedback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FeedbackId { get; set; }
        public string VendorUserId { get; set; }
        public string CustomerUserId { get; set; }
        public string CustomerName { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
