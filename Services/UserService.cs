using NetDynamicPress.Context;
using NetDynamicPress.Models;

namespace NetDynamicPress.Services;

public class UserService : IUserService
{
    PresupuestoContext _context;
    IPasswordService _pwdManager;

    public UserService(PresupuestoContext dbContext, IPasswordService pwdManager)
    {
        _context = dbContext;
        _pwdManager = pwdManager;
    }
    public void TestDatabase()
    {
        _context.Database.EnsureCreated();
    }
    public bool CreateUser(string name, string password) 
    {
        if (_context.Users.Any(p => p.Name == name))
        {
            return false;
        }

        byte[] salt = _pwdManager.GenerateSalt();
        string hashedPassword = _pwdManager.HashPassword(password, salt);

        _context.Users.Add(new User()
        {
            Name = name,
            PasswordHash = hashedPassword,
            PasswordSalt = salt 
        }); // The rest is null
        _context.SaveChanges();

        return true;
    }

    public bool LoginUser(string name, string password)
    {
        User currentUser = _context.Users.Where(p => p.Name == name).FirstOrDefault();

        string storedHash = currentUser.PasswordHash;
        byte[] storedSalt = currentUser.PasswordSalt;

        return _pwdManager.HashPassword(password, storedSalt) == storedHash;
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
    bool CreateUser(string name, string password);
    bool LoginUser(string name, string password);
    User GetFirstUser();
    List<User> GetAllUsers();
}