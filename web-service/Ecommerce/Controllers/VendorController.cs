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
        //[Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> CreateVendorProfile([FromBody] VendorDto vendorDto)
        {
            var vendor = new Vendor
            {
                VendorUserId = vendorDto.VendorUserId,
                PhoneNumber = vendorDto.PhoneNumber,
                Address = vendorDto.Address,
                AverageRating = vendorDto.AverageRating,
            };

            await _vendorService.CreateVendorProfile(vendor);
            return Ok("Vendor profile created.");
        }

        // Update vendor profile
        [HttpPut("{vendorUserId}")]
        //[Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> UpdateVendorProfile(string vendorUserId, [FromBody] VendorDto vendorDto)
        {
            var existingVendor = await _vendorService.GetVendorById(vendorUserId);
            if (existingVendor == null)
            {
                return NotFound("Vendor profile not found.");
            }

            existingVendor.PhoneNumber = vendorDto.PhoneNumber ?? existingVendor.PhoneNumber;
            existingVendor.Address = vendorDto.Address ?? existingVendor.Address;  

            await _vendorService.UpdateVendorProfile(existingVendor);
            return Ok("Vendor profile updated.");
        }

        // Delete vendor profile
        [HttpDelete("{vendorUserId}")]
        //[Authorize(Roles = Roles.Vendor)]
        public async Task<IActionResult> DeleteVendorProfile(string vendorUserId)
        {
            await _vendorService.DeleteVendorProfile(vendorUserId);
            return Ok("Vendor profile deleted.");
        }
    }
}
