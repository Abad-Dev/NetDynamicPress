using Microsoft.AspNetCore.Mvc;
using NetDynamicPress.Models;
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
    public IActionResult CreateUser([FromBody] RegisterViewModel model)
    {
        
        if (_userService.CreateUser(model.Name, model.Email, model.Password))
        {
            return Ok();
        } else {
            return Conflict();
        }
    }

    [HttpPost]
    [Route("/login")]
    public IActionResult LoginUser([FromBody] LoginViewModel model)
    {
        User userFound = _userService.LoginUser(model.Email, model.Password);
        if (userFound != null)
        {
            string Token = _jwtService.GenerateToken(userFound.Id);
            return Ok(Token);
        } else {
            return Unauthorized();
        }
    }
}