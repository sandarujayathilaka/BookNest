using Ecommerce.DataAccess;
using Ecommerce.Models;
using MongoDB.Driver;

namespace Ecommerce.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IMongoCollection<ApplicationUser> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.GetCollection<ApplicationUser>("Users");
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateUser(ApplicationUser user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateUserApprovalStatus(string userId, bool isApproved)
        {
            var filter = Builders<ApplicationUser>.Filter.Eq(u => u.UserId, userId);
            var update = Builders<ApplicationUser>.Update.Set(u => u.IsApproved, isApproved);
            await _users.UpdateOneAsync(filter, update);
        }

        // Get all unapproved users
        public async Task<List<ApplicationUser>> GetUnapprovedUsers()
        {
            return await _users.Find(user => user.IsApproved == false).ToListAsync();
        }
    }
}
