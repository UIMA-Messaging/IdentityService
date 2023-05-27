using IdentityService.Contracts;
using IdentityService.Exceptions;
using IdentityService.Repositories.Users;

namespace IdentityService.Services;

public class UserService
{
    private readonly IUserRepository repository;

    public UserService(IUserRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await repository.GetUserByUsername(username) ?? throw new UserNotFound();
    }

    public async Task<User[]> GetUsersByQuery(string query, int count, int offset)
    {
        var users = await repository.GetUsersByQuery(query, count, offset);
        return users.ToArray();
    }
}
