using Ecommerce.DataAccess;
using Ecommerce.Models;
using Ecommerce.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecommerce.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Ecommerce.Dto;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public UserController(IUserRepository userRepository, JwtService jwtService,UserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _jwtService = jwtService;
        }

        // User Registration 
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var existingUser = await _userRepository.GetUserByEmail(registerUserDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already in use.");
            }

            var user = new ApplicationUser
            {
                FullName = registerUserDto.FullName,
                Email = registerUserDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.PasswordHash),
                Role = registerUserDto.Role,
                IsApproved = registerUserDto.IsApproved
            };

            await _userService.RegisterUser(user);
            return Ok("User registration successful. Please wait for approval.");
        }

        // User Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Ecommerce.Models.LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByEmail(loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            // Only allow login if the user is approved
            if (!user.IsApproved)
            {
                return Forbid("Your account is not approved.");
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        // CSR: Approve user
        [HttpPatch("approve/{userId}")]
        //[Authorize(Roles = Roles.CSR)]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            await _userService.ApproveUser(userId);
            return Ok("User approved.");
        }

        // Admin: Get user by email
        [HttpGet("{email}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetOneUserByEmail(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [HttpGet("unapproved")]
        public async Task<ActionResult<List<ApplicationUser>>> GetUnapprovedUsers()
        {
            var users = await _userService.GetUnapprovedUsers();
            if (users == null || users.Count == 0)
            {
                return NotFound("No unapproved accounts found.");
            }
            return Ok(users);
        }
    }
}
