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
    public IActionResult CreateUser(string name, string email, string password)
    {
        
        if (_userService.CreateUser(name, email, password))
        {
            return Ok();
        } else {
            return Conflict();
        }
    }

    [HttpPost]
    [Route("/login")]
    public IActionResult LoginUser(string email, string password)
    {
        if (_userService.LoginUser(email, password))
        {
            string Token = _jwtService.GenerateToken(email);
            return Ok(Token);
        } else {
            return Unauthorized();
        }
    }
}