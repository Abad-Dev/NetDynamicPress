using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetDynamicPress.Models;
using NetDynamicPress.Services;

namespace NetDynamicPress.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ReciboController : ControllerBase
{
    private readonly IReciboService _reciboService;
    private readonly IJwtService _jwtService;

    public ReciboController(IReciboService reciboService, IJwtService jwtService)
    {
        _reciboService = reciboService;
        _jwtService = jwtService;
    }


    [HttpPost]
    public IActionResult Create([FromBody] Recibo recibo)
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);

        recibo.UserId = userId;
        _reciboService.Create(recibo);
        return Ok(recibo);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recibo>>> GetAll()
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);

        List<Recibo> recibos = await _reciboService.GetByUserId(userId).ToListAsync();
        return Ok(recibos);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Recibo> GetById(string id)
    {
        Recibo recibo = _reciboService.GetById(id);
        if (recibo == null)
        {
            return NotFound(new { Message = "Recibo no encontrado" });
        }

        return Ok(recibo);
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult<Recibo> UpdateRecibo(string id, Recibo recibo)
    {
        Recibo reciboUpdated = _reciboService.UpdateRecibo(id, recibo);
        if (reciboUpdated == null)
        {
            return NotFound(new { Message = "Recibo no encontrado para actualizar" });
        }

        return Ok(reciboUpdated);
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeleteRecibo(string id)
    {
        if (_reciboService.DeleteRecibo(id))
        {
            return Accepted();
        }
        else{
            return BadRequest();
        }
    }
}
