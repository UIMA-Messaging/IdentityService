using ContactApi.Contracts;
using ContactApi.Exceptions;
using ContactApi.Repository;
using ContactApi.Services;

namespace ContactService.Services.Keys;

public class KeyService : IKeyService
{
    private readonly KeyRepository repository;
    private readonly UserService service;
    
    public KeyService(KeyRepository repository, IUserService service)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.service = this.service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<KeyBundle> GetKeyBundle(string from, string to)
    {
        if (from == to)
        {
            throw new BadKeyBundleRequest();
        }
        return await repository.GetKeyBundleAndDisposeFromUser(to);
    }

    public async Task RegisterExchangeKeys(ExchangeKeys keys)
    {
        var _ = await service.GetUserById(keys.UserId) ?? throw new UserNotFound();
        await repository.CreateOrUpdateIdentityKey(keys.UserId, keys.IdentityKey);
        await repository.CreateOrUpdateSignedPreKey(keys.UserId, keys.SignedPreKey);
        await repository.CreateOrUpdateSignature(keys.UserId, keys.Signature);
        await repository.CreateOneTimePreKeys(keys.UserId, keys.OneTimePreKeys);
    }
}