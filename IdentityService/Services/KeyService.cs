using IdentityService.Contracts;
using IdentityService.Exceptions;
using IdentityService.RabbitMQ;
using IdentityService.Repositories.Keys;
using IdentityService.Repositories.Users;

namespace IdentityService.Services.Keys;

public class KeyService
{
    private readonly IKeyRepository keyRepository;
    private readonly IUserRepository userRepository;
    
    public KeyService(
        IKeyRepository keys, 
        IUserRepository users, 
        IRabbitMQListener<ExchangeKeys> keyExchangeListener)
    {
        keyRepository = keys ?? throw new ArgumentNullException(nameof(keys));
        userRepository = users ?? throw new ArgumentNullException(nameof(users));
        keyExchangeListener.OnReceive += (_, keys) => RegisterExchangeKeys(keys);
    }

    public async Task<KeyBundle> GetKeyBundle(string from, string to)
    {
        if (from == to)
        {
            throw new BadKeyBundleRequest();
        }
        _ = await userRepository.GetUserById(from) ?? throw new UserNotFound();
        _ = await userRepository.GetUserById(to) ?? throw new UserNotFound();
        return await keyRepository.GetKeyBundleAndDisposeFromUser(to);
    }

    public async Task RegisterExchangeKeys(ExchangeKeys keys)
    {
        _ = await userRepository.GetUserById(keys.UserId) ?? throw new UserNotFound();
        await keyRepository.CreateOrUpdateKeys(keys.UserId, keys.IdentityKey, keys.SignedPreKey, keys.Signature, keys.OneTimePreKeys);
    }
}