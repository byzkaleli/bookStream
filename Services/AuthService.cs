using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using bookStream.Data;
using bookStream.Models;
using Microsoft.IdentityModel.Tokens;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public string GenerateJwtToken(string userId, string userEmail)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, userEmail),
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateVerificationToken()
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return token;
    }

    public void VerifyEmail(string token)
    {
        var user = _context.Users.SingleOrDefault(u => u.Token == token);
        if (user == null)
        {
            throw new Exception("Geçersiz doğrulama token'ı");
        }

        user.IsEmailConfirmed = true; // E-posta doğrulandı
        user.Token = null; // Token silinir
        _context.SaveChanges();
    }

    public User Register(User model)
    {
        var passwordHash = HashPassword(model.Password);
        var token = GenerateVerificationToken();

        var user = new User
        {
            Username = model.Username,
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            Password = passwordHash,
            IsEmailConfirmed = false,  // E-posta doğrulaması yapılmadı
            Token = token,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public string HashPassword(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }

    public User Login(string email, string password)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == email);
        if (user == null || !VerifyPassword(user.Password, password))
        {
            throw new UnauthorizedAccessException("Geçersiz e-posta veya şifre");
        }

        if (!user.IsEmailConfirmed)
        {
            throw new UnauthorizedAccessException("E-posta adresi doğrulanmamış");
        }

        return user;
    }

    public bool VerifyPassword(string storedPassword, string enteredPassword)
    {
        using (var hmac = new HMACSHA512())
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
            return Convert.ToBase64String(hash) == storedPassword;
        }
    }
}
