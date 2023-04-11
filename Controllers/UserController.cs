using IdentityService.Contracts;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService service;

    public UserController(IUserService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }
    
    [HttpGet("id/{userId}")]
    public async Task<User> GetUserById(string userId)
    {
        return await service.GetUserById(userId);
    }

    [HttpGet("username/{username}")]
    public async Task<User> GetUserByUsername(string username)
    {
        return await service.GetUserByUsername(username);
    }
    
    [HttpGet("displayName/{displayName}")]
    public async Task<User> GetUserByDisplayName(string displayName)
    {
        return await service.GetUserByDisplayName(displayName);
    }
}
