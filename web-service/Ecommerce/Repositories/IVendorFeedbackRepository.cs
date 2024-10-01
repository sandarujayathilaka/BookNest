using Ecommerce.Models;

namespace Ecommerce.Repositories
{
    public interface IVendorFeedbackRepository
    {
        Task AddFeedback(VendorFeedback vendorFeedback);
        Task<List<VendorFeedback>> GetFeedbackByVendor(string vendorUserId);
        //Task UpdateComment(string feedbackId, string newComment);
        Task<VendorFeedback> GetFeedbackById(string feedbackId);
        Task DeleteFeedback(string feedbackId);
        Task UpdateFeedback(VendorFeedback updatedFeedback);

    }
}
