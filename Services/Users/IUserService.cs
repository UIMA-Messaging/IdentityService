using ContactApi.Contracts;

namespace ContactApi.Services;

public interface IUserService
{
    Task<User> GetUserById(string userId);
    Task<User> GetUserByUsername(string username);
    Task<User> GetUserByDisplayName(string displayName);
}