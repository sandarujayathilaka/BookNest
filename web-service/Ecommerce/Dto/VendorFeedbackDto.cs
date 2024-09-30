namespace Ecommerce.Dto
{
    public class VendorFeedbackDto
    {
        public string FeedbackId { get; set; }
        public string VendorUserId { get; set; }
        public string CustomerUserId { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
