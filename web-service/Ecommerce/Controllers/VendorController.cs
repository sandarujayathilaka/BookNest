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

        public VendorController(IVendorRepository vendorRepository, VendorService vendorService)
        {
            _vendorRepository = vendorRepository;
            _vendorService = vendorService;
        }

        // Get all vendors
        [HttpGet]
        public async Task<IActionResult> GetVendors()
        {
            var vendors = await _vendorService.GetAllVendors();
            return Ok(vendors);
        }



        // Create a new vendor profile
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
                AverageRating = vendorDto.AverageRating,
            };

            await _vendorService.CreateVendorProfile(vendor);
            return Ok("Vendor profile created.");
        }

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



        // Delete vendor profile
        [HttpDelete("{vendorUserId}")]
        [Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> DeleteVendorProfile(string vendorUserId)
        {
            await _vendorService.DeleteVendorProfile(vendorUserId);
            return Ok("Vendor profile deleted.");
        }
    }
}
