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

        public async Task<IEnumerable<User>> GetUsersByQuery(string query, int count, int offset)
        {
            await using var connection = factory.GetOpenConnection();
            // following query needs `CREATE EXTENSION pg_trgm;` enabled
            const string sql = @"
                SELECT * 
                FROM Users
                WHERE SIMILARITY(Username, @Query) > 0.3
                    OR SIMILARITY(DisplayName, @Query) > 0.3 
                --ORDER BY SIMILARITY(DisplayName, @Query) 
                --  AND SIMILARITY(Username, @Query) DESC
                LIMIT @Count OFFSET @Offset"; 
            return await connection.QueryAsync<User>(sql, new { Query = query, Count = count, Offset = offset });
        }
    }
}
