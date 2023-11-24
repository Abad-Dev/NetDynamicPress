using Microsoft.AspNetCore.Mvc;
using NetDynamicPress.Models;
using NetDynamicPress.Services;

namespace NetDynamicPress.Controllers;

[ApiController]
[Route("[controller]")]
public class PresupuestoController : ControllerBase
{
    IPresupuestoService _PresupuestoService;

    public PresupuestoController(IPresupuestoService PresupuestoService)
    {
        _PresupuestoService = PresupuestoService;
    }

    [HttpPost]
    public void Create(Presupuesto presupuesto)
    {
        _PresupuestoService.Create(presupuesto);
    }
}