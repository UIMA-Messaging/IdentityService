using ContactService.Contracts;

namespace ContactService.Services.Keys;

public interface IKeyService
{
    Task<KeyBundle> GetKeyBundle(string from, string to);
    Task RegisterExchangeKeys(ExchangeKeys keys);
}