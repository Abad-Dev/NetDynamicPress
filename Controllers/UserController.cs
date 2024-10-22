using Microsoft.AspNetCore.Authorization;
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

    [HttpPost]
    [Route("/register")]
    public IActionResult CreateUser([FromBody] RegisterViewModel model)
    {

        if (_userService.CreateUser(model.Name, model.Email, model.Password))
        {
            return Ok();
        }
        else
        {
            return Conflict("El email no es válido o ya existe");
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
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpGet]
    public IActionResult GetUserByToken()
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);

        User userFound = _userService.GetById(userId);

        if (userFound == null) { return NotFound(); }

        return Ok(userFound);
    }


    [HttpPut]
    [Route("{id}")]
    [Authorize]
    public IActionResult UpdateUser(string id, [FromBody] User user)
    {
        if (_userService.UpdateUser(id, user))
        {
            return Accepted();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPut]
    [Route("UpdateTopImage")]
    [Authorize]
    public async Task<IActionResult> UpdateTopImage([FromForm] UpdateFileViewModel form)
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);

        var result = await _userService.UpdateUserFileAsync(userId, form.file, "topImage");

        if (result)
        {
            Console.WriteLine("Usuario actualizó topImage");
            return Accepted();
        }
        else
        {
            Console.WriteLine("Usuario no actualizó topImage");
            return BadRequest();
        }
    }

    [HttpPut]
    [Route("UpdateSignature")]
    [Authorize]
    public async Task<IActionResult> UpdateSignature([FromForm] UpdateFileViewModel form)
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);

        var result = await _userService.UpdateUserFileAsync(userId, form.file, "signature");

        if (result)
        {
            Console.WriteLine("Usuario actualizó signature");
            return Accepted();
        }
        else
        {
            Console.WriteLine("Usuario no actualizó signature");
            return BadRequest();
        }
    }

}