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
                WITH selected_data AS (
                    SELECT
                        u.identity_key AS IdentityKey,
                        u.signed_pre_key AS SignedPreKey,
                        u.signature AS Signature,
                        o.preKey AS OneTimePreKey,
                        o.id AS one_time_pre_key_id
                    FROM
                        Users u
                        INNER JOIN OneTimePreKeys o ON u.id = o.user_id
                    WHERE
                        u.id = @userId
                    LIMIT 1
                )
                DELETE FROM OneTimePreKeys
                WHERE id IN (SELECT one_time_pre_key_id FROM selected_data)
                RETURNING IdentityKey, SignedPreKey, Signature, OneTimePreKey;";
            return await connection.QueryFirstAsync<KeyBundle>(sql, new { userId });
        }

        public async Task CreateOrUpdateKeys(string userId, string identityKey, string signePreKey, string signature, IEnumerable<string> oneTimePreKeys)
        {
            using var connection = factory.GetOpenConnection();

            const string sql = @"
                INSERT INTO public.Users(
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

            const string preKeysSql= @"
                INSERT INTO public.OneTimePreKeys(id, user_id, pre_key, created_at)
	            VALUES (uuid_generate_v4(), @userId, @preKey, CURRENT_DATE);";

            oneTimePreKeys.ToList().ForEach(async preKey => await connection.ExecuteAsync(preKeysSql, new { userId, preKey }));
        }
    }
}

