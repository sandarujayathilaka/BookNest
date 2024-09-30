using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;


namespace Ecommerce.Services
{
    public class VendorFeedbackService
    {
        private readonly IVendorFeedbackRepository _vendorFeedbackRepository;
        private readonly IVendorRepository _vendorRepository;

        public VendorFeedbackService(IVendorFeedbackRepository vendorFeedbackRepository, IVendorRepository vendorRepository)
        {
            _vendorFeedbackRepository = vendorFeedbackRepository;
            _vendorRepository = vendorRepository;
        }

        //Craete feedback
        public async Task AddFeedback(VendorFeedback vendorFeedback)
        {
           
            vendorFeedback.Id = ObjectId.GenerateNewId().ToString();

            vendorFeedback.FeedbackId = GenerateFeedbackId();

            await _vendorFeedbackRepository.AddFeedback(vendorFeedback);

            await UpdateVendorAverageRating(vendorFeedback.VendorUserId);
        }


        private string GenerateFeedbackId()
        {
            var randomLetters = GenerateRandomLetters(2);
            var randomNumber = new Random().Next(10000, 99999);
            return $"FB{randomLetters}{randomNumber}";
        }

        private string GenerateRandomLetters(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }



        //Get all feedbacks for a vendor
        public async Task<List<VendorFeedback>> GetFeedbackByVendor (string vendorUserId)
        {
            return await _vendorFeedbackRepository.GetFeedbackByVendor(vendorUserId);
        }


        //Update feedback comment
        public async Task UpdateComment(string feedbackId, string newComment)
        {
            await _vendorFeedbackRepository.UpdateComment(feedbackId, newComment);
        }


        private async Task UpdateVendorAverageRating(string vendorUserId)
        {
            var feedbacks = await _vendorFeedbackRepository.GetFeedbackByVendor(vendorUserId);
            if (feedbacks.Count() > 0)
            {
                double averageRating = feedbacks.Average(f => f.Rating);
                // Update vendor's average rating in the Vendor collection (you'll need a method in your VendorRepository for this)
                await _vendorRepository.UpdateVendorAverageRating(vendorUserId, averageRating);
            }
        }


    }
}
