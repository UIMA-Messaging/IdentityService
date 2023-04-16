using ContactService.Contracts;
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
    
    [HttpGet("id/{userId}")]
    public async Task<User> GetUserById(string userId)
    {
        return await service.GetUserById(userId);
    }

    [HttpGet("username/{username}")]
    public async Task<PaginatedResults> GetUserByUsername(string username, [FromQuery] int count = 10, [FromQuery] int offset = 0)
    {
        var results = await service.GetUserByUsername(username, count, offset);

        string protocol = HttpContext.Request.IsHttps ? "https" : "http";
        string host = HttpContext.Request.Host.Value;
        string baseUrl = $@"{protocol}://{host}/users/username/{username}";

        return new PaginatedResults
        {
            NextPage = results.Length < count ? null : @$"{baseUrl}?count={count}&offset={offset + count}",
            PreviousPage = offset - count < 0 ? null : @$"{baseUrl}?count={count}&offset={offset - count}",
            Results = results,
        };
    }
    
    [HttpGet("displayName/{displayName}")]
    public async Task<PaginatedResults> GetUserByDisplayName(string displayName, [FromQuery] int count = 10, [FromQuery] int offset = 0)
    {
        var results = await service.GetUserByDisplayName(displayName, count, offset);

        string protocol = HttpContext.Request.IsHttps ? "https" : "http";
        string host = HttpContext.Request.Host.Value;
        string baseUrl = $@"{protocol}://{host}/users/displayName/{displayName}";

        return new PaginatedResults
        {
            NextPage = results.Length < count ? null : @$"{baseUrl}?count={count}&offset={offset + count}",
            PreviousPage = offset - count < 0 ? null : @$"{baseUrl}?count={count}&offset={offset - count}",
            Results = results,
        };
    }
}
