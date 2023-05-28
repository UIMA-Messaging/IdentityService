using IdentityService.Contracts;
using IdentityService.Contracts;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly UserService service;

    public UserController(UserService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }
    
    [HttpGet("username/{username}")]
    public async Task<User> GetUserByUsername(string username)
    {
        return await service.GetUserByUsername(username);
    }

    [HttpGet("search/{query}")]
    public async Task<PaginatedResults<User>> GetUserByName(string query, [FromQuery] int count, [FromQuery] int offset)
    {
        count = count == default ? 10 : count;

        var results = await service.GetUsersByQuery(query, count, offset);

        string protocol = HttpContext.Request.IsHttps ? "https" : "http";
        string host = HttpContext.Request.Host.Value;
        string baseUrl = $@"{protocol}://{host}/users/search/{query}";

        return new PaginatedResults<User>
        {
            NextPage = results.Length < count ? null : @$"{baseUrl}?count={count}&offset={offset + count}",
            PreviousPage = offset - count < 0 ? null : @$"{baseUrl}?count={count}&offset={offset - count}",
            Results = results,
        };
    }
}
