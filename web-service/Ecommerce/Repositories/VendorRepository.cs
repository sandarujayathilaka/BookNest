using Ecommerce.DataAccess;
using Ecommerce.Models;
using MongoDB.Driver;

namespace Ecommerce.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly IMongoCollection<Vendor> _vendors;

        public VendorRepository(MongoDbContext context)
        {
            _vendors = context.GetCollection<Vendor>("Vendors");
        }

        public async Task CreateVendorProfile(Vendor vendor)
        {
            await _vendors.InsertOneAsync(vendor);
        }

        public async Task UpdateVendorProfile(Vendor vendor)
        {
            await _vendors.ReplaceOneAsync(v => v.VendorUserId == vendor.VendorUserId, vendor);
        }

        public async Task DeleteVendorProfile(string vendorUserId)
        {
            await _vendors.DeleteOneAsync(vendor => vendor.VendorUserId == vendorUserId);
        }

        public async Task<Vendor> GetVendorById(string vendorUserId)
        {
            return await _vendors.Find(v => v.VendorUserId == vendorUserId).FirstOrDefaultAsync();
        }

        public async Task<List<Vendor>> GetAllVendors()
        {
            return await _vendors.Find(v => true).ToListAsync();
        }


        public async Task UpdateAverageRating(string vendorUserId, double newAverageRating)
        {
            var update = Builders<Vendor>.Update.Set(v => v.AverageRating, newAverageRating);
            await _vendors.UpdateOneAsync(v => v.VendorUserId == vendorUserId, update);
        }

        // Update the vendor's average rating
        public async Task UpdateVendorAverageRating(string vendorUserId, double newAverageRating)
        {
            var filter = Builders<Vendor>.Filter.Eq(v => v.VendorUserId, vendorUserId);
            var update = Builders<Vendor>.Update.Set(v => v.AverageRating, newAverageRating);

            await _vendors.UpdateOneAsync(filter, update);
        }


    }
}
