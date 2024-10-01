using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUser(ApplicationUser user)
        {
            user.Id = ObjectId.GenerateNewId().ToString();
            
            user.UserId = GenerateUserId(user.Role);

            await _userRepository.CreateUser(user);
        }

        private string GenerateUserId(string role)
        {
            var prefix = role switch
            {
                "Customer" => "CUS",
                "CSR" => "CSR",
                "Vendor" => "VEN",
                "Admin" => "ADM",
                _ => "GEN" 
            };

            var randomLetters = GenerateRandomLetters(2);
            var randomNumber = new Random().Next(10000, 99999);

            return $"{prefix}{randomLetters}{randomNumber}";
        }

        private string GenerateRandomLetters(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ApplicationUser> GetOneUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task ApproveUser(string userId)
        {
            await _userRepository.UpdateUserApprovalStatus(userId, true);
        }

        public async Task<List<ApplicationUser>> GetUnapprovedUsers()
        {
            return await _userRepository.GetUnapprovedUsers();
        }


        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userRepository.GetUserById(userId);
        }
    }
}
