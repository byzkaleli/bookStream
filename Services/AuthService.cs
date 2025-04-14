using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using bookStream.Data;
using bookStream.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace bookStream.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateJwtToken(string userId, string userEmail, string username) // username parametresi eklendi
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Email, userEmail),
        new Claim(ClaimTypes.Name, username), // Artık hata vermeyecek
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Ek güvenlik için
    };

            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
            {
                throw new InvalidOperationException("JWT Secret Key is not configured properly");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // UTC kullanımı önerilir
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateVerificationToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task VerifyEmailAsync(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == token);
            if (user == null) throw new Exception("Invalid token");

            user.IsEmailConfirmed = true;
            user.Token = null;
            await _context.SaveChangesAsync();
        }

        public async Task<User> RegisterAsync(UserRegisterDto model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                throw new Exception("Username already exists");

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                throw new Exception("Email already registered");

            var user = new User
            {
                Username = model.Username,
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Password = HashPassword(model.Password),
                IsEmailConfirmed = false,
                Token = GenerateVerificationToken(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public string HashPassword(string password)
        {
            using var deriveBytes = new Rfc2898DeriveBytes(password, 16, 10000);
            byte[] salt = deriveBytes.Salt;
            byte[] hash = deriveBytes.GetBytes(20);
            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
        }

        public async Task<UserProfileDto> LoginAsync(UserLoginDto model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !VerifyPassword(user.Password, model.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!user.IsEmailConfirmed)
                throw new UnauthorizedAccessException("Email not verified");

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Role = user.Role,
                IsEmailConfirmed = user.IsEmailConfirmed,
                ProfilePhoto = user.ProfilePhoto
            };
        }

        private bool VerifyPassword(string storedHash, string password)
        {
            var parts = storedHash.Split('|');
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            using var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] newHash = deriveBytes.GetBytes(20);
            return newHash.SequenceEqual(hash);
        }
    }
}