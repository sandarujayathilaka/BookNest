using Ecommerce.Models;

namespace Ecommerce.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmail(string email);
        Task CreateUser(ApplicationUser user);
        Task UpdateUserApprovalStatus(string userId, bool isApproved);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<List<ApplicationUser>> GetUnapprovedUsers();
        Task<List<ApplicationUser>> GetUnactivatedUserProfilesAsync();
        Task<bool> ActivateUserProfileAsync(string userId);
        Task<ApplicationUser> GetUserById(string userId);

    }
}