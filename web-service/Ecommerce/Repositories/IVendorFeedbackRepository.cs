using Ecommerce.Models;

namespace Ecommerce.Repositories
{
    public interface IVendorFeedbackRepository
    {
        Task AddFeedback(VendorFeedback vendorFeedback);
        Task<List<VendorFeedback>> GetFeedbackByVendor(string vendorUserId);
        Task UpdateComment(string feedbackId, string newComment);

    }
}
