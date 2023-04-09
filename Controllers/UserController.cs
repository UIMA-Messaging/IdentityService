using ContactApi.Contracts;
using ContactApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService service;

    public UserController(IUserService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }
    
    [HttpGet("{userId}")]
    public async Task<User> GetUserInfo(string userId)
    {
        return await service.GetUserById(userId);
    }
}
