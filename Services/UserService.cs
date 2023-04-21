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
