using ContactService.Contracts;
using ContactService.Exceptions;
using ContactService.Repository;

namespace ContactService.Services;

public class UserService : IUserService
{
    private readonly UserRepository repository;

    public UserService(UserRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<User> GetUserById(string userId)
    {
        return await repository.GetUserById(userId) ?? throw new UserNotFound();
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await repository.GetUserByUsername(username) ?? throw new UserNotFound();
    }

    public async Task<User> GetUserByDisplayName(string displayName)
    {
        return await repository.GetUserByDisplayName(displayName) ?? throw new UserNotFound();
    }
}
