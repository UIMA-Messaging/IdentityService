using Dapper;
using IdentityService.Contracts;
using IdentityService.Repositories.Connection;

namespace IdentityService.Repositories
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
            await using var connection = factory.GetOpenConnection();
            const string sql = @"
                WITH bundle AS
                (
	                SELECT
		                u.identity_key AS identitykey,
		                u.signed_pre_key AS signedprekey,
		                u.signature AS signature,
		                o.pre_key AS onetimeprekey,
		                o.id AS onetimeprekeyid
	                FROM
		                ""Users"" u
		                INNER JOIN ""OneTimePreKeys"" o ON u.id = o.user_id
	                WHERE
		                u.id = @userId
	                LIMIT 1
                ),
                deleted_keys AS
                (
	                DELETE FROM ""OneTimePreKeys""
	                WHERE id IN (SELECT onetimeprekeyid FROM bundle)
	                RETURNING id
                )
                SELECT 
	                identitykey, 
	                signedprekey, 
	                signature, 
	                onetimeprekey
                FROM 
	                bundle
                WHERE 
	                onetimeprekeyid IN (SELECT id FROM deleted_keys);";
            return await connection.QueryFirstAsync<KeyBundle>(sql, new { userId });
        }

        public async Task CreateOrUpdateKeys(string userId, string identityKey, string signePreKey, string signature, IEnumerable<string> oneTimePreKeys)
        {
            await using var connection = factory.GetOpenConnection();

            const string sql = $@"
                INSERT INTO ""Users""(
	                id, identity_key, signed_pre_key, signature, created_at, updated_at)
	                VALUES (@userId, @identityKey, @signePreKey, @signature, CURRENT_DATE, NULL)
                ON CONFLICT (id) DO UPDATE
                SET
	                identity_key = EXCLUDED.identity_key,
	                signed_pre_key = EXCLUDED.signed_pre_key,
	                signature = EXCLUDED.signature,
	                updated_at = CURRENT_DATE;
                ";

            await connection.ExecuteAsync(sql, new { userId, identityKey, signePreKey, signature });

            //CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

            const string preKeysSql= @"
                INSERT INTO ""OneTimePreKeys""(id, user_id, pre_key, created_at)
	            VALUES (uuid_generate_v4(), @userId, @preKey, CURRENT_DATE);";

            foreach(var preKey in oneTimePreKeys)
            {
                await connection.ExecuteAsync(preKeysSql, new { userId, preKey });
            }
        }
    }
}

