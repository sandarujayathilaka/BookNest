using Ecommerce.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ecommerce.Repositories
{
    public interface IVendorRepository
    {
        Task CreateVendorProfile(Vendor vendor);
        Task UpdateVendorProfile(Vendor vendor);
        Task DeleteVendorProfile(string vendorUserId);
        Task<Vendor> GetVendorById(string vendorUserId);
        Task<List<Vendor>> GetAllVendors();
        Task UpdateVendorAverageRating(string vendorUserId, double newAverageRating);

    }
}
