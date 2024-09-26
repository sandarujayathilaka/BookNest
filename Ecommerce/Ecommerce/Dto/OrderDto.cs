namespace Ecommerce.Dto
{
    public class OrderDto
    {
        
        public string ProductID { get; set; }
        public string CustomerID { get; set; }
        public string VendorID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
