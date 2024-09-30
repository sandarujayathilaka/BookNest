namespace Ecommerce.Dto
{
    public class RegisterUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public Boolean IsApproved { get; set; } = false;

        public Boolean AccountActivated = false;

    }
}
