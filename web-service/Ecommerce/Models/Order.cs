using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string OrderID { get; set; }

        public List<ProductOrder> Products { get; set; } 

        public string CustomerID { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } // "Processing", "Dispatched", "Delivered", "Cancelled"
        public int TotalItems { get; set; }
        public bool OrderCancelation { get; set; } = false;
        public string cancelationNote { get; set; } = "NOT PROVIDED";
        public decimal TotalAmount { get; set; }
        public DateTime LastStatusChange { get; set; }
    }

    public class ProductOrder
    {
        public string ProductID { get; set; }
        public string VendorID { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
