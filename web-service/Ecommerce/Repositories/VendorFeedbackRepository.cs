using Ecommerce.DataAccess;
using Ecommerce.Models;
using MongoDB.Driver;


namespace Ecommerce.Repositories
{
    public class VendorFeedbackRepository : IVendorFeedbackRepository
    {
        private readonly IMongoCollection<VendorFeedback> _vendorFeedbacks;

        public VendorFeedbackRepository(MongoDbContext context)
        {
            _vendorFeedbacks = context.GetCollection<VendorFeedback>("VendorFeedbacks");
        }

        public async Task AddFeedback(VendorFeedback vendorFeedback)
        {
            await _vendorFeedbacks.InsertOneAsync(vendorFeedback);
        }


    }
}
