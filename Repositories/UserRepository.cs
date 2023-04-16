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

        public async Task<IEnumerable<User>> GetUserByUsername(string username, int count, int offset)
        {
            await using var connection = factory.GetOpenConnection();
            const string sql = @"SELECT * FROM Users WHERE Username = @Username LIMIT @Count OFFSET @Offset";
            return await connection.QueryAsync<User>(sql, new { Username = username, Count = count, Offset = offset });
        }

        public async Task<IEnumerable<User>> GetUserByDisplayName(string displayName, int count, int offset)
        {
            await using var connection = factory.GetOpenConnection();
            const string sql = @"SELECT * FROM Users WHERE DisplayName = @DisplayName LIMIT @Count OFFSET @Offset";
            return await connection.QueryAsync<User>(sql, new { DisplayName = displayName, Count = count, Offset = offset });        
        }
    }
}
