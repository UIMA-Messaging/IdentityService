using ContactApi.Contracts;

namespace ContactService.Services.Keys;

public interface IKeyService
{
    public Task<KeyBundle> GetKeyBundle(string from, string to);
    public Task RegisterExchangeKeys(ExchangeKeys keys);
}