using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Vendor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string VendorUserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double AverageRating { get; set; } 




    }
}
