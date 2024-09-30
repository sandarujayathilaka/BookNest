using Ecommerce.Models;


namespace Ecommerce.Repositories
{
    public interface IVendorFeedbackRepository
    {
        Task AddFeedback(VendorFeedback vendorFeedback);
        //Task<IEnumerable<VendorFeedback>> GetFeedbackByVendorId(string VendorId);
        //Task<VendorFeedback> GetFeedbackById(string FeedbackId);
        //Task UpdateFeedback(VendorFeedback vendorFeedback);
        //Task DeleteFeedback(string FeedbackId);

    }
}
