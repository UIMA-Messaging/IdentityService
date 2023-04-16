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
    public async Task<User[]> GetUserByUsername(string username, [FromQuery] int count = 10, [FromQuery] int offset = 0)
    {
        return await service.GetUserByUsername(username, count, offset);
    }
    
    [HttpGet("displayName/{displayName}")]
    public async Task<User[]> GetUserByDisplayName(string displayName, [FromQuery] int count = 10, [FromQuery] int offset = 0)
    {
        return await service.GetUserByDisplayName(displayName, count, offset);
    }
}
