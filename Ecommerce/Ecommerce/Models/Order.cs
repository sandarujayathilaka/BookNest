using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ecommerce.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 
        public string OrderID { get; set; } 
        public string ProductID { get; set; }
        public string CustomerID { get; set; }
        public string VendorID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }  // "Processing", "Dispatched", "Delivered", "Cancelled"
    }
}
