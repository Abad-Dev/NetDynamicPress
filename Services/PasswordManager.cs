using System.Security.Cryptography;

namespace NetDynamicPress.Managers;
public class PasswordManager
{
    public static string HashPassword(string password, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            byte[] hash = pbkdf2.GetBytes(32); // 32 bytes for SHA256
            return Convert.ToBase64String(hash);
        }
    }

    public static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16]; // 16 bytes for a decent salt
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public static bool VerifyPassword(string inputPassword, string storedHash, byte[] salt)
    {
        string inputHash = HashPassword(inputPassword, salt);
        return inputHash == storedHash;
    }
}