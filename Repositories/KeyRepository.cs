using ContactApi.Contracts;
using ContactApi.Repository.Connection;
using Dapper;

namespace ContactApi.Repository
{
    public class KeyRepository
    {
        private readonly IConnectionFactory factory;

        public KeyRepository(IConnectionFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<KeyBundle> GetKeyBundleAndDisposeByUserId(string userId)
        {
            using var connection = factory.GetOpenConnection();
            const string sql = @"
                SELECT *
                FROM Users
                WHERE Id = @Id
                LIMIT 1";
            return await connection.QueryFirstAsync<KeyBundle>(sql, new { Id = userId });
        }
    }
}

