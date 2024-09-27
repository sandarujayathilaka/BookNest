using Ecommerce.Models;

namespace Ecommerce.Dto
{
    public class VendorProductDto
    {
        public string OrderID { get; set; }
        public string CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public ProductOrder Product { get; set; } 
    }
}
