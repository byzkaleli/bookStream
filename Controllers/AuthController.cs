using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using bookStream.Models;
using bookStream.Configurations;
using bookStream.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace bookStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthController> _logger;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly EmailSettings _emailSettings;

        public AuthController(
            IOptions<JwtSettings> jwtSettings,
            IUserRepository userRepository,
            ILogger<AuthController> logger,
            IOptions<EmailSettings> emailSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
            _logger = logger;
            _passwordHasher = new PasswordHasher<User>();
            _emailSettings = emailSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest(Response<object>.ErrorResponse("Username and password are required."));
            }

            var user = await _userRepository.GetUserByUsername(login.Username);
            if (user == null || !user.IsEmailConfirmed)
            {
                _logger.LogWarning("Failed login attempt for user: {Username}", login.Username);
                return Unauthorized(Response<object>.ErrorResponse("Invalid credentials or email not verified."));
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);
            if (result != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Failed login attempt for user: {Username}", login.Username);
                return Unauthorized(Response<object>.ErrorResponse("Invalid credentials."));
            }

            var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(Response<object>.SuccessResponse(new { Token = tokenString }, "Login successful."));
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<User>>> Register(User user)
        {
            if (await _userRepository.GetUserByUsername(user.Username) != null)
                return BadRequest(Response<User>.ErrorResponse("Username already registered"));

            if (await _userRepository.GetUserByEmail(user.Email) != null)
                return BadRequest(Response<User>.ErrorResponse("Email already registered"));

            user.Password = _passwordHasher.HashPassword(user, user.Password);
            user.Token = GenerateVerificationToken();
            user.IsEmailConfirmed = false;

            string verificationLink = $"https://github.com/byzkaleli";
            SendVerificationEmail(user.Email, verificationLink);

            var createdUser = await _userRepository.AddUser(user);
            return Response<User>.SuccessResponse(createdUser, "User registered successfully. Please check your email to verify your account.");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var user = await _userRepository.GetUserByToken(token);
            if (user == null)
            {
                return BadRequest(Response<object>.ErrorResponse("Invalid verification token."));
            }

            user.IsEmailConfirmed = true;
            user.Token = null;
            await _userRepository.UpdateUser(user);

            return Ok(Response<object>.SuccessResponse(null, "Email successfully verified."));
        }

        private string GenerateVerificationToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private void SendVerificationEmail(string email, string verificationLink)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // TLS için 587, SSL için 465
                    Credentials = new NetworkCredential("bookstream1982@gmail.com", "tkev jqql bcqq qtqy"),
                    EnableSsl = true // TLS kullanımı için true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("bookstream1982@gmail.com"),
                    Subject = "Email Verification",
                    Body = $"Click the link to verify your email: <a href='{verificationLink}'>Verify</a>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
                Console.WriteLine($"Verification email sent to {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                throw;
            }
        }

    }
}
