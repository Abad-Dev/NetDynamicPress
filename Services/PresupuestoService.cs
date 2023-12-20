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
        presupuesto.User = null; // It is asigned null to avoid recursion problems (User.Presupuestos.User.Presupuestos...)
        _context.Presupuestos.Add(presupuesto);
        _context.SaveChanges();
    }

    public IQueryable<Presupuesto> GetByUserId(string userId)
    {
        return _context.Presupuestos.Where(p => p.UserId == userId).AsQueryable();
    }

    public Presupuesto GetById(string id)
    {
        return _context.Presupuestos.Find(id);
    }
}

public interface IPresupuestoService
{
    void Create(Presupuesto presupuesto);
    Presupuesto GetById(string id);
    IQueryable<Presupuesto> GetByUserId(string userId);
}