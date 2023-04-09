using ContactApi.Contracts;
using ContactApi.Repository.Connection;
using Dapper;

namespace ContactApi.Repository
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
            using var connection = factory.GetOpenConnection();
            const string sql = @"
                SELECT *
                FROM Users
                WHERE Id = @Id
                LIMIT 1";
            return await connection.QueryFirstAsync<User>(sql, new { Id = userId });
        }
    }
}

