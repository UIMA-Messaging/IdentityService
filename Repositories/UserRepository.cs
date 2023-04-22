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

        public async Task<User> GetUserByUsername(string username)
        {
            await using var connection = factory.GetOpenConnection();
            const string sql = @"SELECT * FROM ""Users"" WHERE Username = @Username LIMIT 1";
            var results = await connection.QueryAsync<User>(sql, new { Username = username });
            return results.FirstOrDefault();
        }

        public async Task<User> GetUserById(string userId)
        {
            await using var connection = factory.GetOpenConnection();
            const string sql = @"SELECT * FROM ""Users"" WHERE Id = @Id LIMIT 1";
            var results = await connection.QueryAsync<User>(sql, new { Id = userId });
            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<User>> GetUsersByQuery(string query, int count, int offset)
        {
            await using var connection = factory.GetOpenConnection();
            const string sql = @"
                SELECT *
                FROM ""Users""
                WHERE SIMILARITY(Username, @Query) > 0.2
	                OR SIMILARITY(DisplayName, @Query) > 0.2 
                ORDER BY (SIMILARITY(DisplayName, @Query), 
                    SIMILARITY(Username, @Query)) DESC
                LIMIT @Count OFFSET @Offset"; 
            return await connection.QueryAsync<User>(sql, new { Query = query, Count = count, Offset = offset });
        }
    }
}
