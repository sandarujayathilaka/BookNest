using Ecommerce.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Services
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _expirationDays;

        public JwtService(IConfiguration config)
        {
            _secret = config.GetSection("Jwt").GetSection("SecretKey").Value;
            _issuer = config.GetSection("Jwt").GetSection("Issuer").Value;
            _audience = config.GetSection("Jwt").GetSection("Audience").Value;
            _expirationDays = Convert.ToDouble(config.GetSection("Jwt").GetSection("ExpirationDays").Value);
        }

        public string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            Console.WriteLine(user.UserId);
            Console.WriteLine(user.Email);
            Console.WriteLine(user.Role);
            Console.WriteLine(key);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.UserId),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_expirationDays),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
