using NetDynamicPress.Context;
using NetDynamicPress.Models;

namespace NetDynamicPress.Services;

public class PresupuestoService : IPresupuestoService
{
    PresupuestoContext _context;

    public PresupuestoService(PresupuestoContext dbContext)
    {
        _context = dbContext;
    }
    public void Create(Presupuesto presupuesto)
    {
        _context.Presupuestos.Add(presupuesto);
        _context.SaveChanges();
    }
}

public interface IPresupuestoService
{
    void Create(Presupuesto presupuesto);
}