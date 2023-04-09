using ContactApi.Contracts;
using ContactApi.Exceptions;
using ContactApi.Repository;

namespace ContactApi.Services;

public class UserService : IUserService
{
    private readonly UserRepository repository;

    public UserService(UserRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(UserRepository));
    }

    public async Task<User> GetUserById(string userId)
    {
        return await repository.GetUserById(userId) ?? throw new UserNotFound();
    }
}