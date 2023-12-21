using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetDynamicPress.Models;
using NetDynamicPress.Services;

namespace NetDynamicPress.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize] // Asegura que todas las acciones en este controlador requieren autenticación
public class PresupuestoController : ControllerBase
{
    private readonly IPresupuestoService _presupuestoService;
    private readonly IJwtService _jwtService;

    public PresupuestoController(IPresupuestoService PresupuestoService, IJwtService jwtService)
    {
        _presupuestoService = PresupuestoService;
        _jwtService = jwtService;
    }

    [HttpPost]
    public IActionResult Create(Presupuesto presupuesto)
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);
        
        presupuesto.UserId = userId;
        _presupuestoService.Create(presupuesto);
        return Ok(presupuesto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Presupuesto>>> GetAll()
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()["Bearer ".Length..];
        string userId = _jwtService.GetUserIdFromToken(token);

        List<Presupuesto> presupuestos = await _presupuestoService.GetByUserId(userId).ToListAsync();
        
        return Ok(presupuestos);      
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Presupuesto> GetOne(string id)
    {
        Presupuesto presupuesto = _presupuestoService.GetById(id);
        return Ok(presupuesto);
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult<Presupuesto> UpdatePresupuesto(string id, Presupuesto presupuesto)
    {
        Presupuesto presupuestoUpdated = _presupuestoService.UpdatePresupuesto(id, presupuesto);
        return Ok(presupuestoUpdated);
    }
}