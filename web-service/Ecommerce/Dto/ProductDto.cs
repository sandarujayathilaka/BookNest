namespace Ecommerce.Dto
{
    public class ProductDto
    {
        public string ProductID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ImageDto Image { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public string VendorID { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string DeniedMessage { get; set; }
    }

    public class CreateProductDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ImageDto Image { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
