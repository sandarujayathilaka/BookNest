namespace Ecommerce.Dto
{
    public class ProductDto
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public string VendorID { get; set; }
    }
}
