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
    public bool CreateUser(string name, string email, string password) 
    {
        if (_context.Users.Any(p => p.Email == email))
        {
            return false;
        }

        byte[] salt = _pwdManager.GenerateSalt();
        string hashedPassword = _pwdManager.HashPassword(password, salt);

        User newUser = new()
        {
            Name = name,
            Email = email,
            PasswordHash = hashedPassword,
            PasswordSalt = salt 
        };// The rest is null

        _context.Users.Add(newUser); 
        _context.SaveChanges();
        return true;
    }

    public User LoginUser(string email, string password)
    {
        User currentUser = _context.Users.Where(p => p.Email == email).FirstOrDefault();

        string storedHash = currentUser.PasswordHash;
        byte[] storedSalt = currentUser.PasswordSalt;

        if (_pwdManager.HashPassword(password, storedSalt) == storedHash)
        {
            return currentUser;
        } else {
            return null;
        }
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
    bool CreateUser(string name, string email, string password);
    User LoginUser(string name, string password);
    User GetFirstUser();
    List<User> GetAllUsers();
}