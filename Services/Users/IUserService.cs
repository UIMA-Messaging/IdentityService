using ContactApi.Contracts;

namespace ContactApi.Services;

public interface IUserService
{
    public Task<User> GetUserById(string userId);
}