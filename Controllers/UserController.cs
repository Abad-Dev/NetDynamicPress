using Microsoft.AspNetCore.Mvc;
using NetDynamicPress.Services;

namespace NetDynamicPress.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    IUserService _userService;

    public UserController(IUserService UserService)
    {
        _userService = UserService;
    }

    [HttpGet]
    public void TestDatabase()
    {
        _userService.TestDatabase();
    }
}