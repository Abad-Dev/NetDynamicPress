using NetDynamicPress.Context;

namespace NetDynamicPress.Services;

public class UserService : IUserService
{
    PresupuestoContext _context;

    public UserService(PresupuestoContext dbContext)
    {
        _context = dbContext;
    }
    public void TestDatabase()
    {
        _context.Database.EnsureCreated();
    }
}

public interface IUserService
{
    void TestDatabase();
}