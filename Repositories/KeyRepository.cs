using IdentityService.Contracts;
using IdentityService.Repository.Connection;
using Dapper;

namespace IdentityService.Repository
{
    public class KeyRepository
    {
        private readonly IConnectionFactory factory;

        public KeyRepository(IConnectionFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<KeyBundle> GetKeyBundleAndDisposeFromUser(string userId)
        {
            using var connection = factory.GetOpenConnection();
            const string sql = @"
                SELECT *
                FROM Users
                WHERE Id = @Id
                LIMIT 1";
            return await connection.QueryFirstAsync<KeyBundle>(sql, new { Id = userId });
        }

        public async Task CreateOrUpdateIdentityKey(string userId, string key)
        {
            
        }

        public async Task CreateOneTimePreKeys(string userId, string[] keys)
        {
            
        }

        public async Task CreateOrUpdateSignedPreKey(string userId, string key)
        {
            
        }

        public async Task CreateOrUpdateSignature(string userId, string signature)
        {
            
        }
    }
}

