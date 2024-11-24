using NetDynamicPress.Context;
using NetDynamicPress.Models;

namespace NetDynamicPress.Services;
public class ReciboService : IReciboService
{
    PresupuestoContext _context;

    public ReciboService(PresupuestoContext dbContext)
    {
        _context = dbContext;
    }
    public void Create(Recibo recibo)
    {
        Recibo newRecibo = new()
        {
            UserId = recibo.UserId,
            Config = recibo.Config,
            Name = recibo.Name
        };
        _context.Recibos.Add(newRecibo);
        _context.SaveChanges();
    }

    public IQueryable<Recibo> GetByUserId(string userId)
    {
        return _context.Recibos
            .Where(p => p.UserId == userId)
            .AsQueryable();
    }

    public Recibo GetById(string id)
    {
        return _context.Recibos.Find(id);
    }

    public Recibo UpdateRecibo(string reciboId, Recibo recibo)
    {
        Recibo reciboFound = GetById(reciboId);

        if (reciboFound == null)
        {
            return null;
        }

        reciboFound.Config = recibo.Config;
        _context.SaveChanges();

        return reciboFound;
    }

    public bool DeleteRecibo(string id)
    {
        Recibo reciboFound = GetById(id);

        if (reciboFound == null)
        {
            return false;
        }

        _context.Recibos.Remove(reciboFound);
        _context.SaveChanges();
        return true;
    }
}


public interface IReciboService
{
    void Create(Recibo recibo);
    Recibo GetById(string id);
    IQueryable<Recibo> GetByUserId(string userId);
    public Recibo UpdateRecibo(string presupuestoId, Recibo recibo);
    public bool DeleteRecibo    (string id);
}
