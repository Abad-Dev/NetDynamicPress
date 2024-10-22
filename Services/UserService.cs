using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NetDynamicPress.Context;
using NetDynamicPress.Models;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Amazon.Runtime;

namespace NetDynamicPress.Services;

public class UserService : IUserService
{
    PresupuestoContext _context;
    IPasswordService _pwdManager;
    IAmazonS3 _s3Client;
    IConfiguration _config;

    public UserService(PresupuestoContext dbContext, IPasswordService pwdManager, IConfiguration config)
    {
        _context = dbContext;
        _pwdManager = pwdManager;
        _config = config;

        // Crear cliente de S3 con credenciales de configuración
        var credentials = new BasicAWSCredentials(_config["AWS:AccessKeyId"], _config["AWS:SecretAccessKey"]);
        var awsOptions = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_config["AWS:Region"])
        };
        _s3Client = new AmazonS3Client(credentials, awsOptions);
    }
    public bool CreateUser(string name, string email, string password)
    {
        if (_context.Users.Any(p => p.Email == email))
        {
            return false;
        }

        if (!IsValidEmail(email))
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


    public User GetById(string userId)
    {
        User userFound = _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefault();
        if (userFound == null)
        {
            return null;
        }
        userFound.PasswordHash = null;
        userFound.PasswordSalt = null;

        return userFound;
    }

    public bool UpdateUser(string id, User user)
    {
        User userFound = _context.Users
            .Where(u => u.Id == id)
            .FirstOrDefault(); // Usa esta forma cuando necesita el usuario completo

        //userFound.TopImage = user.TopImage;
        //userFound.Signature = user.Signature;
        _context.SaveChanges();

        return true;
    }

    public User LoginUser(string email, string password)
    {
        User currentUser = _context.Users.Where(p => p.Email == email).FirstOrDefault();

        if (currentUser == null)
        {
            return null;
        }
        string storedHash = currentUser.PasswordHash;
        byte[] storedSalt = currentUser.PasswordSalt;
        if (_pwdManager.VerifyPassword(password, storedHash, storedSalt))
        {
            return GetById(currentUser.Id);
        }
        else
        {
            return null;
        }
    }
    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public bool UpdateUserFile(string id, IFormFile topImage, string field)
    {
        if (topImage == null || topImage.Length == 0) { return false; }

        User userFound = _context.Users
            .Where(u => u.Id == id)
            .FirstOrDefault();

        if (userFound == null) { return false; }

        using (var memoryStream = new MemoryStream())
        {
            topImage.CopyTo(memoryStream);

            if (field == "topImage")
            {
                //userFound.TopImage = memoryStream.ToArray();
            }
            else if (field == "signature")
            {
                //userFound.Signature = memoryStream.ToArray();
                Console.WriteLine($"Tamaño del archivo: {topImage.Length} bytes");
            }
        }

        _context.SaveChanges();
        return true;
    }

    public async Task<bool> UpdateUserFileAsync(string userId, IFormFile file, string field)
    {
        if (file == null || file.Length == 0) return false;

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return false;

        string bucketName = _config["AWS:BucketName"];
        string key = $"{userId}/{field}/{file.FileName}";
        string baseUrl = _config["AWS:BaseUrl"];

        // Subir archivo a S3
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var uploadRequest = new PutObjectRequest
            {
                InputStream = memoryStream,
                BucketName = bucketName,
                Key = key,
                ContentType = file.ContentType,
                //CannedACL = S3CannedACL.PublicRead
            };

            await _s3Client.PutObjectAsync(uploadRequest);
        }

        // Guardar la URL correspondiente
        string url = $"{baseUrl}{key}";
        if (field == "topImage")
        {
            user.TopImageUrl = url;
        }
        else if (field == "signature")
        {
            user.SignatureUrl = url;
        }

        _context.SaveChanges();
        return true;
    }


}

public interface IUserService
{
    bool CreateUser(string name, string email, string password);
    User GetById(string userId);
    bool UpdateUser(string id, User user);
    User LoginUser(string name, string password);
    bool IsValidEmail(string email);
    bool UpdateUserFile(string id, IFormFile firma, string field);
    Task<bool> UpdateUserFileAsync(string userId, IFormFile file, string field);
}