using ContactService.Exceptions;
using ContactService.Contracts;
using ContactService.Services.Keys;
using Microsoft.AspNetCore.Mvc;

namespace ContactService.Controllers;

[ApiController]
[Route("keys")]
public class KeyController : ControllerBase
{
    private readonly IKeyService service;

    public KeyController(IKeyService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }
    
    [HttpPost("register/exchanges")]
    public async Task RegisterKeyBundles([FromBody] ExchangeKeys keys)
    {
        await service.RegisterExchangeKeys(keys);
    }
    
    [HttpPost("bundle/{from}/{to}")]
    public async Task<KeyBundle> FetchKeyBundle(string from, string to)
    {
        return await service.GetKeyBundle(from, to);
    }
}