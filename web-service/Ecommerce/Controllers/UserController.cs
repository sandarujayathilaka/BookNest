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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

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
            };
       

            await _userService.RegisterUser(user, registerUserDto.PasswordHash);
            return Ok("User registration successful. Please wait for approval.");
        }

        // User Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Ecommerce.Models.LoginRequest loginRequest)
        {
            var result = await _userService.LoginService(loginRequest.Email, loginRequest.Password);

            if (result.IsSuccess)
            {
                return Ok(new { Token = result.Token });
            }
            else if (result.ErrorMessage == "Unauthorized")
            {
                return Unauthorized("Invalid email or password.");
            }
            else if (result.ErrorMessage == "Forbid")
            {
                return Forbid("Your account is not approved.");
            }
           
            // Generate JWT token
            //var token = _jwtService.GenerateToken(user);
            //return Ok(new { Token = token });

            return BadRequest("An error occurred during login.");
        }


        // CSR: Approve user
        [HttpPatch("approve/{userId}")]
        //[Authorize(Roles = Roles.CSR)]
        public async Task<IActionResult> ApproveUser(string userId, UserApproveDto userDto)
        {
            await _userService.ApproveUser(userId,userDto.FullName,userDto.Email);
            return Ok("User approved.");
        }

        // Admin: Get user by email
        [HttpGet("{email}")]
        //[Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetOneUserByEmail(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        // Admin: Get all users
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }

            return Ok(users);

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


        // GET: api/User/unactivated
        [HttpGet("unactivated")]
        public async Task<ActionResult<List<ApplicationUser>>> GetUnactivatedUserProfiles()
        {
            var users = await _userService.GetUnactivatedUserProfilesAsync();
            if (users == null || users.Count == 0)
            {
                return NotFound("No unactivated user profiles found.");
            }

            return Ok(users);
        }

        [HttpPut("{userId}/activate")]
        public async Task<IActionResult> ActivateUserProfile(string userId)
        {
            bool result = await _userService.ActivateUserProfileAsync(userId);

            if (result)
            {
                return Ok(new { message = "User profile activated successfully" });
            }

            return NotFound(new { message = "User not found" });
        }



        // Get user by ID
        [HttpGet("getuser{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }


    }
}
