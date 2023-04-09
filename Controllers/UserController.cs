using ContactApi.Contracts;
using ContactApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private IUserService service;

    public UserController(IUserService service)
    {
        this.service = service;
    }
    
    [HttpGet("{userId}")]
    public async Task<User> GetUserInfo(string userId)
    {
        return await service.GetUserById(userId);
    }
}
