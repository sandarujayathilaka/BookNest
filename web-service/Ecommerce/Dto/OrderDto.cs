namespace Ecommerce.Dto
{
    public class OrderDto
    {
        public List<ProductOrderDto> Products { get; set; } 
        public string CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }

    public class ProductOrderDto
    {
        public string ProductID { get; set; }
        public string VendorID { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; } 
        public decimal UnitPrice { get; set; } 
        public decimal TotalAmount { get; set; }
    }
}
