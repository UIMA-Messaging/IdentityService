using IdentityService.Contracts;
using IdentityService.Exceptions;
using IdentityService.Repository;

namespace IdentityService.Services;

public class UserService
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

    public async Task<User[]> GetUserByUsername(string username, int count, int offset)
    {
        var users = await repository.GetUserByUsername(username, count, offset);
        return users.ToArray();
    }

    public async Task<User[]> GetUserByDisplayName(string displayName, int count, int offset)
    {
        var users = await repository.GetUserByDisplayName(displayName, count, offset) ?? throw new UserNotFound();
        return users.ToArray();
    }
}
