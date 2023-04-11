using IdentityService.Contracts;
using IdentityService.Repository.Connection;
using Dapper;

namespace IdentityService.Repository
{
    public class UserRepository
    {
        private readonly IConnectionFactory factory;

        public UserRepository(IConnectionFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<User> GetUserById(string userId)
        {
            await using var connection = factory.GetOpenConnection();
            const string sql = @"SELECT * FROM Users WHERE Id = @Id LIMIT 1";
            return await connection.QueryFirstAsync<User>(sql, new { Id = userId });
        }

        public async Task<User> GetUserByUsername(string username)
        {
            await using var connection = factory.GetOpenConnection();
            const string sql = @"SELECT * FROM Users WHERE Username = @Username LIMIT 1";
            return await connection.QueryFirstAsync<User>(sql, new { Username = username });
        }

        public async Task<User> GetUserByDisplayName(string displayName)
        {
            await using var connection = factory.GetOpenConnection();
            const string sql = @"SELECT * FROM Users WHERE DisplayName = @DisplayName LIMIT 1";
            return await connection.QueryFirstAsync<User>(sql, new { DisplayName = displayName });        
        }
    }
}
