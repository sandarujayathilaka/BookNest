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

        public async Task<List<VendorFeedback>> GetFeedbackByVendor(string vendorUserId)
        {
            var feedbacks = await _vendorFeedbacks.Find(f => f.VendorUserId == vendorUserId).ToListAsync();
            return feedbacks;
        }

        public async Task UpdateComment(string feedbackId, string newComment)
        {
            var filter = Builders<VendorFeedback>.Filter.Eq(f => f.FeedbackId, feedbackId);
            var update = Builders<VendorFeedback>.Update.Set(f => f.Comment, newComment);
            await _vendorFeedbacks.UpdateOneAsync(filter, update);
        }






    }
}
