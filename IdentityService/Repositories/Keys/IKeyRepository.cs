using IdentityService.Contracts;

namespace IdentityService.Repositories.Keys
{
    public interface IKeyRepository
    {
        Task<KeyBundle> GetKeyBundleAndDisposeFromUser(string userId);

        Task CreateOrUpdateKeys(string userId, string identityKey, string signePreKey, string signature, IEnumerable<string> oneTimePreKeys);
    }
}