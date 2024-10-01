using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories;
using Ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly VendorService _vendorService;
        private readonly IUserRepository _userRepository; // Inject IUserRepository

        public VendorController(IVendorRepository vendorRepository, VendorService vendorService, IUserRepository userRepository)
        {
            _vendorRepository = vendorRepository;
            _vendorService = vendorService;
            _userRepository = userRepository; // Initialize the user repository
        }

        // Get all vendors
        [HttpGet]
        public async Task<IActionResult> GetVendors()
        {
            var vendors = await _vendorService.GetAllVendors();
            var vendorDtos = new List<VendorDto>();

            foreach (var vendor in vendors)
            {
                // Fetch user details based on VendorUserId
                var user = await _userRepository.GetUserById(vendor.VendorUserId);

                // Combine vendor and user details into VendorDto
                var vendorDto = new VendorDto
                {
                    VendorUserId = vendor.VendorUserId,
                    Address = vendor.Address,
                    FullName = user?.FullName,
                    Email = user?.Email,
                    PhoneNumber = vendor.PhoneNumber
                };

                vendorDtos.Add(vendorDto);
            }

            return Ok(vendorDtos);
        }



        // Vendor : Create a new vendor profile
        [HttpPost]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> CreateVendorProfile([FromBody] VendorDto vendorDto)
        {
            // Get the VendorUserId from the logged-in user's claims
            var vendorUserId = User.FindFirst("UserId")?.Value;
            Console.WriteLine(vendorUserId);

            if (vendorUserId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var vendor = new Vendor
            {
                VendorUserId = vendorUserId,
                PhoneNumber = vendorDto.PhoneNumber,
                Address = vendorDto.Address,
                AverageRating = 0,
            };

            await _vendorService.CreateVendorProfile(vendor);
            return Ok("Vendor profile created.");
        }


        //Vendor : Update own profile

        [HttpPut]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> UpdateVendorProfile([FromBody] VendorDto vendorDto)
        {
            // Get the VendorUserId from the token (claims)
            var loggedInUserId = User.FindFirst("UserId")?.Value;

            if (loggedInUserId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Retrieve the vendor profile using the logged-in user's ID 
            var existingVendor = await _vendorService.GetVendorById(loggedInUserId);
            if (existingVendor == null)
            {
                return NotFound("Vendor profile not found.");
            }

            // Update the vendor profile
            existingVendor.PhoneNumber = vendorDto.PhoneNumber ?? existingVendor.PhoneNumber;
            existingVendor.Address = vendorDto.Address ?? existingVendor.Address;

            await _vendorService.UpdateVendorProfile(existingVendor);
            return Ok("Vendor profile updated successfully.");
        }



        // Vendor : Delete vendor profile
        [HttpDelete("{vendorUserId}")]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> DeleteVendorProfile(string vendorUserId)
        {
            await _vendorService.DeleteVendorProfile(vendorUserId);
            return Ok("Vendor profile deleted.");
        }
    }
}
