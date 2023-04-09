using ContactApi.Contracts;
using ContactApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers;

[ApiController]
[Route("keys")]
public class KeyController : ControllerBase
{
    [HttpPost("bundle/{from}/{to}")]
    public IActionResult FetchKeyBundle(string from, string to)
    {
        if (from == to)
        {
            throw new BadKeyBundleRequest();
        }
        
        return Ok();
    }

    [HttpPost("register/exchanges")]
    public IActionResult RegisterKeyBundles([FromBody] ExchangeKeys keys)
    {
        return Ok();
    }
}