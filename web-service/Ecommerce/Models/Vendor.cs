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
        public string VendorId { get; set; } //check again need or not
        public string UserId { get; set; } // Link to the user who is a vendor
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double AverageRating { get; set; } = 0;
        public bool IsActive { get; set; } = true;


        //public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();// Products owned by the vendor
        //public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new HashSet<ProductCategory>();// Categories created by the vendor
        //public virtual ICollection<VendorFeedback> Feedbacks { get; set; } = new HashSet<VendorFeedback>();// Feedback received to the vendor



    }
}
