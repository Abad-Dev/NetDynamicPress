using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDynamicPress.Models;
using NetDynamicPress.Services;

namespace NetDynamicPress.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize] // Asegura que todas las acciones en este controlador requieren autenticación
public class PresupuestoController : ControllerBase
{
    private readonly IPresupuestoService _PresupuestoService;
    private readonly IJwtService _jwtService;

    public PresupuestoController(IPresupuestoService PresupuestoService, IJwtService jwtService)
    {
        _PresupuestoService = PresupuestoService;
        _jwtService = jwtService;
    }

    [HttpGet]
    [Route("/test")]
    public IActionResult Test()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
        {
            string token = authorizationHeader["Bearer ".Length..];
            string userId = _jwtService.GetUserIdFromToken(token);
            
            return Ok(userId);
        }

        return null;
    }

    [HttpPost]
    public IActionResult Create(Presupuesto presupuesto)
    {
        _PresupuestoService.Create(presupuesto);
        return Ok(presupuesto);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetOne(string id)
    {
        Presupuesto presupuesto = _PresupuestoService.GetById(id);
        return Ok(presupuesto);
    }

}