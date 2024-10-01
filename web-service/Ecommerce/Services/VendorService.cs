using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;

namespace Ecommerce.Services
{
    public class VendorService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task CreateVendorProfile(Vendor vendor)
        {
            vendor.Id = ObjectId.GenerateNewId().ToString();
            await _vendorRepository.CreateVendorProfile(vendor);

        }

    

        public async Task UpdateVendorProfile(Vendor vendor)
        {
            Console.WriteLine(vendor.Id);
            await _vendorRepository.UpdateVendorProfile(vendor);

        }



        public async Task DeleteVendorProfile(string vendorUserId)
        {
            await _vendorRepository.DeleteVendorProfile(vendorUserId);
        }



        public async Task<Vendor> GetVendorById(string vendorUserId)
        {
            return await _vendorRepository.GetVendorById(vendorUserId);
        }



        public async Task<IEnumerable<Vendor>> GetAllVendors()
        {
           return await _vendorRepository.GetAllVendors();
        }



    }
}
