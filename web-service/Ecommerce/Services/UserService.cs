using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, EmailService emailService, JwtService jwtService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        public async Task RegisterUser(ApplicationUser user,string password)
        {
            user.Id = ObjectId.GenerateNewId().ToString();

            user.UserId = GenerateUserId(user.Role);

            if (user.Role == "CSR" || user.Role == "Vendor")
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Your Account Registration Details",
                    $"Dear User,\n\nYour account has been successfully created.\n\nYour password is: {password}\n\nPlease change your password after logging in."
                );
            }

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

        public async Task ApproveUser(string userId, string fullname,string email)
        {
            await _userRepository.UpdateUserApprovalStatus(userId, true);
            string subject = "Account Approval Notification";
            string body = $"Dear {fullname},\n\nYour account has been approved and is now active.\n\nThank you!";

            await _emailService.SendEmailAsync(email, subject, body);
        }

        public async Task<List<ApplicationUser>> GetUnapprovedUsers()
        {
            return await _userRepository.GetUnapprovedUsers();

        }

        public async Task<List<ApplicationUser>> GetUnactivatedUserProfilesAsync()
        {
            return await _userRepository.GetUnactivatedUserProfilesAsync();
        }

        public async Task<bool> ActivateUserProfileAsync(string userId)
        {
            return await _userRepository.ActivateUserProfileAsync(userId);
        }

        public async Task<LoginResult> LoginService(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Unauthorized"
                };
            }

            if (!user.IsApproved)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Forbid"
                };
            }

            var token = _jwtService.GenerateToken(user);

            return new LoginResult
            {
                IsSuccess = true,
                Token = token
            };
        }


        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userRepository.GetUserById(userId);
        }



    }
}
