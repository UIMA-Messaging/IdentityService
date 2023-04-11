using IdentityService.Contracts;

namespace IdentityService.Services;

public interface IUserService
{
    Task<User> GetUserById(string userId);
    Task<User> GetUserByUsername(string username);
    Task<User> GetUserByDisplayName(string displayName);
}