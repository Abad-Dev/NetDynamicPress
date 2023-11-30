using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetDynamicPress.Services;

namespace NetDynamicPress.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public UserController(IUserService UserService, IJwtService jwtService)
    {
        _userService = UserService;
        _jwtService = jwtService;
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

    [HttpPost]
    [Route("/login")]
    public IActionResult LoginUser(string name, string password)
    {
        string Token = _jwtService.GenerateToken(name);
        if (_userService.LoginUser(name, password))
        {
            return Ok(Token);
        } else {
            return Unauthorized();
        }
    }
}