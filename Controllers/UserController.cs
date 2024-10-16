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
        } else {
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
        } else {
            return Unauthorized();
        }
    }
    
    [HttpGet]
    public IActionResult GetUserByToken()
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);
        User userFound = _userService.GetById(userId);

        if (userFound != null)
        {
            var userResponse = new
            {
                userFound.Email,
                TopImage = Convert.ToBase64String(userFound.TopImage),
                Signature = Convert.ToBase64String(userFound.Signature),
            };
            return Ok(userFound);
        } else
        {
            return BadRequest();
        }
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
    public IActionResult updateFirma([FromForm] UpdateFileViewModel form) 
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);
        
        if (_userService.UpdateTopImage(userId, form.file))
        {
            return Accepted();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPut]
    [Route("UpdateSignature")]
    [Authorize]
    public IActionResult updateSignature([FromForm] UpdateFileViewModel form)
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);
        if (_userService.UpdateSignature(userId, form.file))
        {
            return Accepted();
        }
        else
        {
            return BadRequest();
        }
    }

    

}