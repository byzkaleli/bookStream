using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using bookStream.Configurations;
using bookStream.Models;
using bookStream.Repositories;
using bookStream.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace bookStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthController> _logger;
        private readonly EmailSettings _emailSettings;

        public AuthController(
            AuthService authService,
            IUserRepository userRepository,
            ILogger<AuthController> logger,
            IOptions<EmailSettings> emailSettings)
        {
            _authService = authService;
            _userRepository = userRepository;
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto login)
        {
            if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest(Response<object>.ErrorResponse("Username and password are required."));
            }

            try
            {
                // AuthService üzerinden login işlemi
                var userProfile = await _authService.LoginAsync(login);

                // Token oluşturma
                var token = _authService.GenerateJwtToken(userProfile.Id.ToString(), userProfile.Email, userProfile.Username);
                var token = _authService.GenerateJwtToken(userProfile.Id.ToString(), userProfile.Email);

                return Ok(Response<object>.SuccessResponse(new
                {
                    Token = token
                }, "Login successful."));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Login failed for user: {Username}", login.Username);
                return Unauthorized(Response<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user: {Username}", login.Username);
                return StatusCode(500, Response<object>.ErrorResponse("An error occurred during login."));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                var user = await _authService.RegisterAsync(registerDto);
                var verificationLink = $"{Request.Scheme}://{Request.Host}/api/auth/confirm-email?token={user.Token}";
                //SendVerificationEmail(user.Email, verificationLink);

                user = new User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Role = user.Role,
                    IsEmailConfirmed = user.IsEmailConfirmed
                };

                return Ok(Response<User>.SuccessResponse(user, "Registration successful. Please check your email."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed");
                return BadRequest(Response<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            try
            {
                await _authService.VerifyEmailAsync(token);
                return Ok(Response<object>.SuccessResponse(null, "Email verified successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email verification failed");
                return BadRequest(Response<object>.ErrorResponse(ex.Message));
            }
        }

        private void SendVerificationEmail(string email, string verificationLink)
        {
            try
            {
                using var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
                {
                    Port = _emailSettings.Port,
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                    EnableSsl = _emailSettings.UseSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromAddress),
                    Subject = "Email Verification",
                    Body = $"Please verify your email by clicking this link: <a href='{verificationLink}'>{verificationLink}</a>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send verification email");
                throw;
            }
        }
    }
}