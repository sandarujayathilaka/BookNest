using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ecommerce.Models
{

    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public string VendorID { get; set; }

    }


}
