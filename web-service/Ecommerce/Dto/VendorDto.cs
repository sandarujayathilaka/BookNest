namespace Ecommerce.Dto
{
    //check this again
    public class VendorDto
    {
        public int VendorId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public double AverageRating { get; private set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
