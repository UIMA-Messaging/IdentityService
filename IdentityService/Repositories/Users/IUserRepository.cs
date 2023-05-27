using IdentityService.Contracts;

namespace IdentityService.Repositories.Users
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);

        Task<User> GetUserById(string userId);

        Task<IEnumerable<User>> GetUsersByQuery(string query, int count, int offset);
    }
}