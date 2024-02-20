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
        Presupuesto newPesupuesto = new() {
            UserId = presupuesto.UserId,
            Config = presupuesto.Config,
            Name = presupuesto.Name
        };
        _context.Presupuestos.Add(newPesupuesto);
        _context.SaveChanges();
    }

    public IQueryable<Presupuesto> GetByUserId(string userId)
    {
        return _context.Presupuestos
            .Where(p => p.UserId == userId)
            .AsQueryable();
    }

    public Presupuesto GetById(string id)
    {
        return _context.Presupuestos.Find(id);
    }

    public Presupuesto UpdatePresupuesto(string presupuestoId, Presupuesto presupuesto)
    {
        Presupuesto presupuestoFound = GetById(presupuestoId);

        if (presupuestoFound == null)
        {
            return null;
        }

        presupuestoFound.Name = presupuesto.Name; 
        presupuestoFound.Config = presupuesto.Config;
        _context.SaveChanges();

        return presupuestoFound;
    }

    public bool DeletePresupuesto(string id)
    {
        Presupuesto presupuestoFound = GetById(id);

        if (presupuestoFound == null)
        {
            return false;
        }

        _context.Presupuestos.Remove(presupuestoFound);
        _context.SaveChanges();
        return true;
    }
}

public interface IPresupuestoService
{
    void Create(Presupuesto presupuesto);
    Presupuesto GetById(string id);
    IQueryable<Presupuesto> GetByUserId(string userId);
    public Presupuesto UpdatePresupuesto(string presupuestoId, Presupuesto presupuesto);
    public bool DeletePresupuesto(string id);
}