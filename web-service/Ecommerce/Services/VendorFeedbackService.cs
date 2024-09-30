using Ecommerce.Models;
using Ecommerce.Repositories;
using MongoDB.Bson;


namespace Ecommerce.Services
{
    public class VendorFeedbackService
    {
        private readonly IVendorFeedbackRepository _vendorFeedbackRepository;

        public VendorFeedbackService(IVendorFeedbackRepository vendorFeedbackRepository)
        {
            _vendorFeedbackRepository = vendorFeedbackRepository;
        }

        //Craete feedback
        public async Task AddFeedback(VendorFeedback vendorFeedback)
        {
           
            vendorFeedback.Id = ObjectId.GenerateNewId().ToString();

            vendorFeedback.FeedbackId = GenerateFeedbackId();

            await _vendorFeedbackRepository.AddFeedback(vendorFeedback);

            //await UpdateVendorAverageRating(vendorFeedback.VendorId);
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


    }
}
