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

    public async Task<User[]> GetUsersByQuery(string query, int count, int offset)
    {
        var users = await repository.GetUsersByQuery(query, count, offset);
        return users.ToArray();
    }
}
