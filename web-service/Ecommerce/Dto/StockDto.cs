namespace Ecommerce.Dto
{
    public class StockDto
    {
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public int LowStockThreshold { get; set; }
        public bool IsLowStockAlert { get; set; }
        public string VendorId { get; set; }
    }
}
