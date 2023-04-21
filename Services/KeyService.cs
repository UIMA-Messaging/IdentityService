using IdentityService.Contracts;
using IdentityService.Exceptions;
using IdentityService.Repository;

namespace IdentityService.Services.Keys;

public class KeyService
{
    private readonly KeyRepository keyRepository;
    private readonly UserRepository userRepository;
    
    public KeyService(KeyRepository keys, UserRepository users)
    {
        this.keyRepository = keys ?? throw new ArgumentNullException(nameof(keys));
        this.userRepository = users ?? throw new ArgumentNullException(nameof(users));
    }

    public async Task<KeyBundle> GetKeyBundle(string from, string to)
    {
        if (from == to)
        {
            throw new BadKeyBundleRequest();
        }
        return await keyRepository.GetKeyBundleAndDisposeFromUser(to);
    }

    public async Task RegisterExchangeKeys(ExchangeKeys keys)
    {
        var _ = await userRepository.GetUserById(keys.UserId) ?? throw new UserNotFound();
        await keyRepository.CreateOrUpdateKeys(keys.UserId, keys.IdentityKey, keys.SignedPreKey, keys.Signature, keys.OneTimePreKeys);
    }
}