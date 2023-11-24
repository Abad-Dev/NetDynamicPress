using NetDynamicPress.Context;
using NetDynamicPress.Models;

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

    public User GetFirstUser()
    {
        User firstUser = _context.Users.FirstOrDefault();

        return firstUser;
    }

    public List<User> GetAllUsers()
    {
        List<User> Users = _context.Users.ToList();

        return Users;
    }
}

public interface IUserService
{
    void TestDatabase();
    User GetFirstUser();
    List<User> GetAllUsers();
}