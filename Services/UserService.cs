using NetDynamicPress.Context;
using NetDynamicPress.Models;
using NetDynamicPress.Managers;

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
    public bool CreateUser(string name, string password) 
    {
        if (_context.Users.Any(p => p.Name == name))
        {
            return false;
        }

        byte[] salt = PasswordManager.GenerateSalt();
        string hashedPassword = PasswordManager.HashPassword(password, salt);

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

        byte[] storedSalt = currentUser.PasswordSalt;
        string storedHash = currentUser.PasswordHash;

        return PasswordManager.VerifyPassword(password, storedHash, storedSalt);
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