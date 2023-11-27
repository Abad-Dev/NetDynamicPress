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

    [HttpPost]
    public IActionResult CreateUser(string name, string password)
    {
        if (_userService.CreateUser(name, password))
        {
            return Ok();
        } else {
            return Conflict();
        }
    }

    [HttpGet]
    [Route("/login")]
    public IActionResult LoginUser(string name, string password)
    {
        if (_userService.LoginUser(name, password))
        {
            return Ok();
        } else {
            return Unauthorized();
        }
    }
}